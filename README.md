# mba-es-25-grupo-02_backend
Repositório para o Backend do Bootcamp



--------------------------------------------------------------------------------------------------
API: https://azfunc-backend.azurewebsites.net/api/AzFunc_CadastroUser
Metodos: "post", "put", "delete"

POST JSON Req:
    {
        "userlogin" : "user1@mail.com",
        "userpass" : "pass123",
        "userdoctype" : "CPF",
        "userdocnum" : "22222222222",
        "usertype" : "P"
    }

POST Output: Strings não formatadas:
    "Informações Faltantes." = userLogin OU userPass OU userType OU userDocType OU userDocNum sem valor.
    "Usuário ou Senha incorretos." = userLogin OU userDocNum já existentes.
    "Usuário criado." = Novo usuário criado com sucess.
    "Method Put: Em construção." = Solicitado o metodo PUT, ainda em construção.
    "Method Delete: Em construção." = Solicitado o metodo delete, ainda em construção.

--------------------------------------------------------------------------------------------------


 >> Tabela Pessoa <<

* Campos:

>> Tabela Pessoa <<

* Campos:

long 		Pessoa_ID  
string 		Nome  
string		TipoPessoa  
string 		TipoDoc
string 		Documento  
string 		CEP  
string 		Endereco  
int 		Numero  
string 		Complemento  
string 		Bairro  
string 		Cidade  
string 		Estado  
string 		Telefone  
string 		Email  
string 		Senha  
		
* APIs:
. Cadastro;
. Consulta;
. Atualização;

DB
    [userID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [userLogin]     VARCHAR (50)  NOT NULL,
    [userPass]      VARCHAR (32)  NOT NULL,
    [userDocType]   VARCHAR (4)   NOT NULL,
    [userDocNum]    VARCHAR (14)  NOT NULL,
    [userType]      CHAR (1)      NOT NULL,
    [userName]      VARCHAR (50)  NULL,
    [userAddCEP]    VARCHAR (9)   NULL,
    [userAddStreet] VARCHAR (MAX) NULL,
    [userAddNum]    INT           NULL,
    [userAddComp]   VARCHAR (50)  NULL,
    [userAddNeib]   VARCHAR (50)  NULL,
    [userAddCity]   VARCHAR (50)  NULL,
    [userAddState]  VARCHAR (3)   NULL,
    [userTel]       VARCHAR (20)  NULL,



>> Tabela Produto <<

long 	Produto_ID 
string 	Descricao  
long 	Quantidade 
decimal ValorPorKG 
Pessoa 	mProdutor  
		
* APIs:
. Cadastro;
. Consulta;
. Atualização;
. Exclusão;

		
>> Tabela Pedido <<
	
long 		 Pedido_ID     
Produto 	 mProduto      
Pessoa 		 mCliente      
StatusPedido mStatusPedido 
long 		 Quantidade 	
decimal 	 ValorPorKG 	
decimal 	 ValorTotal 	

* APIs:
. Cadastro;
. Consulta;
. Atualização;

--------------------------------------------------------------------------------------------------





REFs:
Expose serverless APIs from HTTP endpoints using Azure API Management:
    https://learn.microsoft.com/en-us/azure/azure-functions/functions-openapi-definition\

Youtube: Getting started with Azure SQL and C#
    https://www.youtube.com/watch?v=2qW1zsuJ9s0

Youtube: Building a Serverless REST API With Azure Functions From Scratch
    https://www.youtube.com/watch?v=3HZjmYohlgc
    * Pay attention to Key Vault on: 
        Function APP: Identity and Configuration
        On VSCode: host.json and local.settings.json to enter the Key Vault
    



Criação do BackEnd (https://docs.google.com/document/d/1BcVYFqXGJF8-xPUav4lJsLPo749dkjuUL27a3Cpnb6c/edit?usp=sharing)
1- Criar o recurso Function APP "AzFunc-BackEnd"

2- Criar o repositório no GitHub "mba-es-25-grupo-02_backend"
* Não esquecer de compartilhar os acessos.

3- Clonar o novo repositório no VSCode

4- Criando uma Function:
4.1- No VSCode, certificar que a extensão "Azure Functions" e suas dependências estão instaladas. 
4.2- No menu do Azure, posicione o mouse sobre "WORKSPACE" e o ícone do Azure Functions APP irá aparecer. Clique nele e depois em "Create Function…"
Siga as instruções:
1o Select the folder that will contain your function project: Selecione o diretório local onde o repositório do Github está clonado.
2o Select a language: C#
3o Select a .NET runtime: .NET 6.0 LTS
OBS: os items 2o e 3o, devem seguir as mesmas configurações utilizadas na criação do recurso Function APP "AzFunc-BackEnd", no portal do Azure.
4o Select a template for your project's first function: Por hora estamos utilizando "HTTP trigger"
5o Provide a function name: AzFunc-CadastroUser (sugestão de nomenclatura: AzFunc-"ModuloSubmodulo")
6o Provide a namespace "Company.Function": Bootcamp.AzFunc
7o AccessRights: Function

4.3 No menu "WORKSPACE", clique em "Run build task to update this list…"
4.4 No menu "Source Control", adicione um comentario, faça o "Commit" e depois "Sync Changes"
Neste momento, criamos uma Function compatível o Functions APP, utilizando o template do Azure e sincronizamos nosso código com o nosso repositório GitHub.

5 Linkando nosso recurso Functions APP "AzFunc-BackEnd" com nosso repositório Github "mba-es-25-grupo-02_backend"
5.1 No portal do azure, encontre o resource "AzFunc-BackEnd", na opção "Deployment Center", escola a "Source", "Signed in as", "Repository" e "Branch".
5.2 Clique no botão "Save", na parte superior da janela.
Neste momento o Azure está criando uma pipeline no "Actions" do Github, importa o codigo para o Function APP, já criando a Function.
5.3 Como o Azure cria o arquivo YML,  dentro do diretório ".github/workflows", precisamos sincronizar o VSCode.

Agora qualquer alteração feita, commit e sync com o GitHub, será replicada para o Azure.
