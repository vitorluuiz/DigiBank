CREATE DATABASE DIGIBANK

USE DIGIBANK

CREATE TABLE Condicoes(
idCondicao TINYINT PRIMARY KEY IDENTITY,
Condicao VARCHAR(20) UNIQUE NOT NULL,
)
GO

CREATE TABLE Usuarios(
idUsuario SMALLINT PRIMARY KEY IDENTITY,
NomeCompleto VARCHAR(100) NOT NULL,
Apelido VARCHAR (20),
CPF CHAR(11) UNIQUE NOT NULL,
Telefone CHAR(11) UNIQUE,
Email VARCHAR(255),
Senha VARCHAR(16) NOT NULL,
PontosVantagem DECIMAL,
Saldo DECIMAL,
RendaFixa DECIMAL,
)
GO

CREATE TABLE AcoesOptions(
idAcaoOption TINYINT PRIMARY KEY IDENTITY,
IndiceConfiabilidade DECIMAL NOT NULL,
IndiceDividendos DECIMAL NOT NULL,
IndiceValorizacao DECIMAL NOT NULL,
Nome VARCHAR(75) UNIQUE NOT NULL,
Descricao VARCHAR (200) NOT NULL,
Codigo VARCHAR(6) UNIQUE NOT NULL,
Dividendos DECIMAL NOT NULL,
CotasDisponiveis SMALLINT NOT NULL,
AcaoImg VARCHAR(255)
)
GO

CREATE TABLE Acoes(
idAcao SMALLINT PRIMARY KEY IDENTITY,
idUsuario SMALLINT FOREIGN KEY REFERENCES Usuarios(idUsuario),
idAcoesOptions TINYINT FOREIGN KEY REFERENCES AcoesOptions(idAcaoOption),
QntCotas TINYINT NOT NULL,
DataAquisicao DATETIME NOT NULL,
ValorInicial DECIMAL NOT NULL
)
GO

CREATE TABLE EmprestimosOptions(
idEmprestimoOption TINYINT PRIMARY KEY IDENTITY,
Valor DECIMAL NOT NULL,
TaxaJuros DECIMAL NOT NULL,
RendaMinima DECIMAL NOT NULL
)
GO

CREATE TABLE Emprestimos(
idEmprestimo SMALLINT PRIMARY KEY IDENTITY,
idUsuario SMALLINT FOREIGN KEY REFERENCES Usuarios(idUsuario),
idCondicao TINYINT FOREIGN KEY REFERENCES Condicoes(idCondicao),
idEmprestimoOptions TINYINT FOREIGN KEY REFERENCES EmprestimosOptions(idEmprestimoOption),
DataInicial DATETIME NOT NULL,
DataFinal DATETIME NOT NULL,
)
GO

CREATE TABLE TiposFundos(
idTipoFundo TINYINT PRIMARY KEY IDENTITY,
TipoFundo VARCHAR(30) UNIQUE NOT NULL
)
GO

CREATE TABLE FundosOptions(
idFundosOption SMALLINT PRIMARY KEY IDENTITY,
idTipoFundo TINYINT FOREIGN KEY REFERENCES TiposFundos(idTipoFundo),
IndiceConfiabilidade DECIMAL NOT NULL,
IndiceDividendos DECIMAL NOT NULL,
IndiceValorizacao DECIMAL NOT NULL,
Dividendos DECIMAL NOT NULL,
TaxaJuros DECIMAL NOT NULL
)
GO

CREATE TABLE Fundos(
idFundo SMALLINT PRIMARY KEY IDENTITY,
idUsuario SMALLINT FOREIGN KEY REFERENCES Usuarios(idUsuario),
idfundosOptions SMALLINT FOREIGN KEY REFERENCES FundosOptions(idFundosOption),
DepositoInicial DECIMAL NOT NULL,
DataInicio DATETIME NOT NULL,
DataFinal DATETIME NOT NULL
)
GO

CREATE TABLE Transacoes(
idTransacao SMALLINT PRIMARY KEY IDENTITY,
idUsuarioPagante SMALLINT FOREIGN KEY REFERENCES Usuarios(idUsuario),
idUsuarioRecebente SMALLINT FOREIGN KEY REFERENCES Usuarios(idUsuario),
Valor DECIMAL NOT NULL,
DataTransacao DATETIME,
Descricao VARCHAR(200)
)
GO

CREATE TABLE Produtos(
idProduto TINYINT PRIMARY KEY IDENTITY,
idUsuario SMALLINT FOREIGN KEY REFERENCES USuarios(idUsuario),
Valor DECIMAL NOT NULL,
Nome VARCHAR(40) NOT NULL,
Descricao VARCHAR (200),
ProdutoImg VARCHAR(255)
)
GO

CREATE TABLE Avaliacoes(
idAvaliacao SMALLINT PRIMARY KEY IDENTITY,
idProduto TINYINT FOREIGN KEY REFERENCES Produtos(idProduto),
Nota DECIMAL NOT NULL,
Comentario VARCHAR(200)
)
GO