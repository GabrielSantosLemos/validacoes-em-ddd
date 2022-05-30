# Pureza do modelo de dom�nio versus integridade do modelo de dom�nio (DDD Trilema)

## Completude do modelo de dom�nio

Neste artigo, falaremos sobre um trilema que surge em cada projeto. Na verdade, recebi cerca de uma d�zia de perguntas sobre esse trilema durante o �ltimo ano ou dois (um pouco embara�oso perceber quanto tempo algumas ideias de artigos passam na minha fila de reda��o).

Para melhor descrever esse trilema, precisamos dar um exemplo. Digamos que temos um sistema de gerenciamento de usu�rios com um caso de uso at� agora: alterar o e-mail do usu�rio. Veja como a Userclasse de dom�nio se parece:

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

E este � o controlador que orquestra este caso de uso:


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

Este � um exemplo de um modelo de dom�nio rico: todas as regras de neg�cios (tamb�m conhecidas como l�gica de dom�nio) est�o localizadas nas classes de dom�nio. Existe uma regra atualmente - que s� podemos atribuir ao usu�rio um e-mail que perten�a ao dom�nio corporativo da empresa desse usu�rio. N�o h� como o c�digo do cliente ignorar essa invari�vel � uma marca registrada de um modelo de dom�nio altamente encapsulado.

Tamb�m podemos dizer que nosso modelo de dom�nio est� completo .Um modelo de dom�nio completo � um modelo que cont�m toda a l�gica de dom�nio do aplicativo. Em outras palavras, n�o h� fragmenta��o da l�gica de dom�nio.

A fragmenta��o da l�gica de dom�nio ocorre quando a l�gica de dom�nio reside em camadas diferentes da camada de dom�nio. No nosso exemplo, oUserController(que pertence � camada de servi�os de aplica��o) n�o cont�m tal l�gica, serve apenas como coordenador entre a camada de dom�nio e o banco de dados.

## Pureza do modelo de dom�nio

Digamos agora que precisamos implementar outra regra de neg�cio: antes de alterar o e-mail do usu�rio, o sistema deve verificar se o novo e-mail j� est� em uso.

Aqui est� uma maneira comum de verificar a exclusividade do e-mail:

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

Isso faz o trabalho, mas esta solu��o introduz a fragmenta��o da l�gica de dom�nio. A camada de dom�nio n�o cont�m mais todas as regras de neg�cio, uma delas passou para o controlador. Agora tamb�m � poss�vel alterar o e-mail do usu�rio sem primeiro verificar sua exclusividade, o que significa que nosso modelo de dom�nio n�o est� totalmente encapsulado.

Existe uma maneira de restaurar a integridade do modelo de dom�nio?

H�. Podemos mover a responsabilidade de verificar a exclusividade do email dentro da Userclasse, assim:

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

Esta vers�o elimina a fragmenta��o do modelo de dom�nio, mas � custa de outra propriedade importante: pureza do modelo de dom�nio .Um modelo de dom�nio puro � um modelo que n�o alcan�a depend�ncias fora do processo. Para ser puro, as classes de dom�nio devem depender apenas de tipos primitivos ou de outras classes de dom�nio.

Em nosso exemplo, perdemos a pureza porque o Usernow fala com o banco de dados. E n�o, substituir UserRepositorypor uma IUserRepositoryinterface n�o ajudar�:

```c#:

public Result ChangeEmail(string newEmail, IUserRepository repository)

```

```c#:

public Result ChangeEmail(string newEmail, Func<string, bool> isEmailUnique)

```

Ambas as alternativas ainda fazem a Userclasse alcan�ar o banco de dados e, portanto , n�o trazem de volta a pureza do modelo de dom�nio .

� da� que vem a escolha entre a integridade e a pureza do modelo de dom�nio. Voc� n�o pode ter os dois ao mesmo tempo.

## O trilema

...

Qual � o melhor?

Eu recomendo fortemente que voc� escolha a pureza do modelo de dom�nio sobre a integridade do modelo de dom�nio, e v� com a terceira abordagem: dividir o processo de tomada de decis�o entre a camada de dom�nio e os controladores. A fragmenta��o da l�gica de dom�nio � um mal menor do que mesclar as responsabilidades de modelagem e comunica��o de dom�nio com depend�ncias fora do processo.

A l�gica de neg�cios � a parte mais importante do aplicativo. � tamb�m a parte mais complexa dela. Mistur�-lo com a responsabilidade adicional de falar com depend�ncias fora do processo faz com que a complexidade dessa l�gica cres�a ainda mais. Evite isso ao m�ximo. A camada de dom�nio deve ser isenta de todas as responsabilidades que n�o sejam a pr�pria l�gica de dom�nio.

Dividir o processo de tomada de decis�o entre a camada de dom�nio e os controladores � a abordagem para a qual a Programa��o Funcional, o Teste de Unidade e (possivelmente) o Design Orientado ao Dom�nio convergem, embora por raz�es diferentes.

- O DDD defende essa abordagem porque ajuda a manter a complexidade do aplicativo gerenci�vel. Como voc� sabe, o DDD � sobre como lidar com a complexidade no cora��o do software , onde "cora��o" significa o modelo de dom�nio.

- A Programa��o Funcional escolhe essa abordagem porque � a �nica maneira de tornar suas fun��es puras. A Programa��o Funcional tem tudo a ver com transpar�ncia referencial e evitar entradas e sa�das ocultas no n�cleo funcional do seu aplicativo (consultar o banco de dados, tamb�m conhecido como E/S do banco de dados, � uma dessas entradas ocultas).

- O Teste de Unidade defende isso porque o modelo de dom�nio puro significa modelo de dom�nio test�vel. Sem a separa��o entre l�gica de neg�cios e comunica��o com depend�ncias fora do processo, seus testes ser�o muito mais dif�ceis de manter, pois voc� ter� que configurar mocks e stubs e, em seguida, verificar as intera��es com eles.

Em nosso projeto de exemplo, dividir o processo de tomada de decis�o entre a camada de dom�nio e os controladores significa colocar a verifica��o de exclusividade de email na classe em UserControllervez da User.