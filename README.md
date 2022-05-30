# Pureza do modelo de domínio versus integridade do modelo de domínio (DDD Trilema)

## Completude do modelo de domínio

Neste artigo, falaremos sobre um trilema que surge em cada projeto. Na verdade, recebi cerca de uma dúzia de perguntas sobre esse trilema durante o último ano ou dois (um pouco embaraçoso perceber quanto tempo algumas ideias de artigos passam na minha fila de redação).

Para melhor descrever esse trilema, precisamos dar um exemplo. Digamos que temos um sistema de gerenciamento de usuários com um caso de uso até agora: alterar o e-mail do usuário. Veja como a Userclasse de domínio se parece:

```c#:

public class User : Entity
{
    public Company Company { get; private set; }
    public string Email { get; private set; }

    public Result ChangeEmail(string newEmail)
    {
        if (Company.IsEmailCorporate(newEmail) == false)
            return Result.Failure("Incorrect email domain");

        Email = newEmail;

        return Result.Success();
    }
}

public class Company : Entity
{
    public string DomainName { get; }

    public bool IsEmailCorporate(string email)
    {
        string emailDomain = email.Split('@')[1];
        return emailDomain == DomainName;
    }
}


```

E este é o controlador que orquestra este caso de uso:


```c#:

public class UserController
{
    public string ChangeEmail(int userId, string newEmail)
    {
        User user = _userRepository.GetById(userId);

        Result result = user.ChangeEmail(newEmail);
        if (result.IsFailure)
            return result.Error;

        _userRepository.Save(user);

        return "OK";
    }
}

```

Este é um exemplo de um modelo de domínio rico: todas as regras de negócios (também conhecidas como lógica de domínio) estão localizadas nas classes de domínio. Existe uma regra atualmente - que só podemos atribuir ao usuário um e-mail que pertença ao domínio corporativo da empresa desse usuário. Não há como o código do cliente ignorar essa invariável — uma marca registrada de um modelo de domínio altamente encapsulado.

Também podemos dizer que nosso modelo de domínio está completo .Um modelo de domínio completo é um modelo que contém toda a lógica de domínio do aplicativo. Em outras palavras, não há fragmentação da lógica de domínio.

A fragmentação da lógica de domínio ocorre quando a lógica de domínio reside em camadas diferentes da camada de domínio. No nosso exemplo, oUserController(que pertence à camada de serviços de aplicação) não contém tal lógica, serve apenas como coordenador entre a camada de domínio e o banco de dados.

## Pureza do modelo de domínio

Digamos agora que precisamos implementar outra regra de negócio: antes de alterar o e-mail do usuário, o sistema deve verificar se o novo e-mail já está em uso.

Aqui está uma maneira comum de verificar a exclusividade do e-mail:

```c#:

// UserController
public string ChangeEmail(int userId, string newEmail)
{
    /* The new validation */
    User existingUser = _userRepository.GetByEmail(newEmail);
    if (existingUser != null && existingUser.Id != userId)
        return "Email is already taken";

    User user = _userRepository.GetById(userId);

    Result result = user.ChangeEmail(newEmail);
    if (result.IsFailure)
        return result.Error;

    _userRepository.Save(user);

    return "OK";
}

```

Isso faz o trabalho, mas esta solução introduz a fragmentação da lógica de domínio. A camada de domínio não contém mais todas as regras de negócio, uma delas passou para o controlador. Agora também é possível alterar o e-mail do usuário sem primeiro verificar sua exclusividade, o que significa que nosso modelo de domínio não está totalmente encapsulado.

Existe uma maneira de restaurar a integridade do modelo de domínio?

Há. Podemos mover a responsabilidade de verificar a exclusividade do email dentro da Userclasse, assim:

```c#:

// User
public Result ChangeEmail(string newEmail, UserRepository repository)
{
    if (Company.IsEmailCorporate(newEmail) == false)
        return Result.Failure("Incorrect email domain");

    User existingUser = repository.GetByEmail(newEmail);
    if (existingUser != null && existingUser != this)
        return Result.Failure("Email is already taken");

    Email = newEmail;

    return Result.Success();
}

// UserController
public string ChangeEmail(int userId, string newEmail)
{
    User user = _userRepository.GetById(userId);

    Result result = user.ChangeEmail(newEmail, _userRepository);
    if (result.IsFailure)
        return result.Error;

    _userRepository.Save(user);

    return "OK";
}

```

Esta versão elimina a fragmentação do modelo de domínio, mas à custa de outra propriedade importante: pureza do modelo de domínio .Um modelo de domínio puro é um modelo que não alcança dependências fora do processo. Para ser puro, as classes de domínio devem depender apenas de tipos primitivos ou de outras classes de domínio.

Em nosso exemplo, perdemos a pureza porque o Usernow fala com o banco de dados. E não, substituir UserRepositorypor uma IUserRepositoryinterface não ajudará:

```c#:

public Result ChangeEmail(string newEmail, IUserRepository repository)

```

```c#:

public Result ChangeEmail(string newEmail, Func<string, bool> isEmailUnique)

```

Ambas as alternativas ainda fazem a Userclasse alcançar o banco de dados e, portanto , não trazem de volta a pureza do modelo de domínio .

É daí que vem a escolha entre a integridade e a pureza do modelo de domínio. Você não pode ter os dois ao mesmo tempo.

## O trilema

...

Qual é o melhor?

Eu recomendo fortemente que você escolha a pureza do modelo de domínio sobre a integridade do modelo de domínio, e vá com a terceira abordagem: dividir o processo de tomada de decisão entre a camada de domínio e os controladores. A fragmentação da lógica de domínio é um mal menor do que mesclar as responsabilidades de modelagem e comunicação de domínio com dependências fora do processo.

A lógica de negócios é a parte mais importante do aplicativo. É também a parte mais complexa dela. Misturá-lo com a responsabilidade adicional de falar com dependências fora do processo faz com que a complexidade dessa lógica cresça ainda mais. Evite isso ao máximo. A camada de domínio deve ser isenta de todas as responsabilidades que não sejam a própria lógica de domínio.

Dividir o processo de tomada de decisão entre a camada de domínio e os controladores é a abordagem para a qual a Programação Funcional, o Teste de Unidade e (possivelmente) o Design Orientado ao Domínio convergem, embora por razões diferentes.

- O DDD defende essa abordagem porque ajuda a manter a complexidade do aplicativo gerenciável. Como você sabe, o DDD é sobre como lidar com a complexidade no coração do software , onde "coração" significa o modelo de domínio.

- A Programação Funcional escolhe essa abordagem porque é a única maneira de tornar suas funções puras. A Programação Funcional tem tudo a ver com transparência referencial e evitar entradas e saídas ocultas no núcleo funcional do seu aplicativo (consultar o banco de dados, também conhecido como E/S do banco de dados, é uma dessas entradas ocultas).

- O Teste de Unidade defende isso porque o modelo de domínio puro significa modelo de domínio testável. Sem a separação entre lógica de negócios e comunicação com dependências fora do processo, seus testes serão muito mais difíceis de manter, pois você terá que configurar mocks e stubs e, em seguida, verificar as interações com eles.

Em nosso projeto de exemplo, dividir o processo de tomada de decisão entre a camada de domínio e os controladores significa colocar a verificação de exclusividade de email na classe em UserControllervez da User.