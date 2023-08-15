USE DIGIBANK

INSERT INTO Condicoes(Condicao)
VALUES ('� pagar'),('Pago'),('Em atraso'),('Extendido')
SELECT * FROM Condicoes
GO

INSERT INTO Usuarios(NomeCompleto, Apelido, CPF, Telefone, Email, Senha, DigiPoints, RendaFixa, Saldo)
VALUES 
('Administrador', 'ADM', '99999999909', '08004040100', 'SAC@digibank.com', '$2a$12$ir1TB.1e463lUaqed9jFWOoIqkTmRLonakE64C6uOPJtV74B9F42a', 100000, 15000, 1000000),
('Vitor Luiz de Carvalho', 'Vitor', '12345678909', '1112345678', 'vitor.sesi21@gmail.com', '$2a$12$ir1TB.1e463lUaqed9jFWOoIqkTmRLonakE64C6uOPJtV74B9F42a', 100, 7000, 10000)
SELECT * FROM Usuarios
GO

INSERT INTO TipoInvestimentos(TipoInvestimento)
VALUES ('Poupan�a'),('Renda Fixa'),('A��es'), ('Fundos de investimentos'), ('Cripto')
SELECT * FROM TipoInvestimentos
GO

INSERT INTO AreaInvestimento(Area)
VALUES ('Poupan�a'),('CDB'),('Institui��o financeira'),('Fundo Imobili�rio residencial'),('Criptomoeda'),('NFT')
SELECT * FROM AreaInvestimento
GO

INSERT INTO InvestimentoOptions(idTipoInvestimento, idAreaInvestimento, Colaboradores, Fundacao, Abertura, Sede, Fundador, Nome, Descricao, Sigla, Logo, MainImg, MainColorHex, PercentualDividendos, ValorAcao, Tick, QntCotasTotais)
Values (1, 1, 1,'21/09/2022', '21/09/2022', 'Bras�lia, Brasil', 'Banco central do Brasil', 'Poupan�a', 'Invista seu dinheiro na boa e velha poupan�a Digibank', 'POUPEC','Poupanca.png', 'Poupanca.png', 'C00414', 0.75,0, '06/05/2022', 1),
(2,2, 1,'21/09/2022', '06/06/2023', 'Bras�lia, Brasil', 'Governo do Brasil', 'CDB', 'Certificado de dep�sito banc�rio', 'CDBBRA','CDB.png', 'CDB.png', 'FFFFFF', 1.13, 500, '06/05/2022', 30000),
(3,3,20045,'21/09/2022', '06/06/2023', 'S�o Paulo, Brasil', 'Everaldo Martins Magalh�es', 'DigiBank Corporation', 'Compre um peda�inho de n�s. Participe do n�cleo de investidores apoiadores da DigiBank, fazendo parte de nosso futuro a partir do capital aberto mundial', 'DIGIBK','Logo.exe', 'Banner.exe', 'C00414', 1.13, 420.3, '06/05/2022', 2500000),
(4,4,36046,'21/09/2022', '06/06/2023', 'Florianopoles, Brasil', 'Matheus Leal Palmuti', 'Sammaleur Entinel SC', 'O maior fundo de Investimentos comerciais de Santa Catarina','CSESC','CSESC.png', 'SESC.png', 'FFFFFF', 0.67, 352.5, '06/03/2023', 2200000),
(5,5,1,'21/09/2022', '06/06/2023', 'Bras�lia, Brasil', 'Banco central do Brasil', 'Criptomoeda do Brasil', 'Moeda virtual do governo do Brasil', 'BRACON','BRACON.png', 'BRACON.png', 'FFFFFF', 0, 102103.6, '06/05/2022', 1000000)
SELECT * FROM InvestimentoOptions
GO

INSERT INTO Investimento(idUsuario, idInvestimentoOption, QntCotas, DataAquisicao, DepositoInicial, isEntrada)
VALUES (1, 1, 1, '29/09/2022', 70500, 'true'), (1, 1, 2, '27/12/2022', 20130, 'false'),
(1, 2, 50, '06/02/2023', 20000, 'true'), (1, 2, 20, '10/06/2023', 500, 'false'),
(1, 3, 3, '21/09/2022', 60.35, 'false'), (1, 3, 2, '25/09/2022', 60.35, 'true'),
(1, 4, 20, '06/02/2023', 50030.4, 'true'), (1, 4, 10, '16/02/2023', 50041.3, 'false'),
(1, 5, 1, '06/02/2022', 32400, 'true'), (1, 5, 10, '06/02/2023', 324000, 'true')
SELECT * FROM Investimento
GO

INSERT INTO Metas(idUsuario, Titulo, ValorMeta, Arrecadado)
VALUES (1 , 'Bicicleta Nova', 900, 450)
SELECT * FROM Metas
GO

INSERT INTO EmprestimosOptions(Valor, TaxaJuros, RendaMinima, PrazoEstimado)
VALUES (1000, 4.30, 0, 30),
(2000, 5.50, 0, 60),
(5000, 5.55, 800, 90),
(10000, 3.33, 2000, 210),
(20000, 3.66, 3500, 365),
(50000, 2.52, 4500, 730),
(75000, 2.21, 5000, 1095)
SELECT * FROM EmprestimosOptions
GO

SELECT * FROM Condicoes
SELECT * FROM Usuarios
SELECT * FROM TipoInvestimentos
SELECT * FROM InvestimentoOptions
SELECT * FROM Investimento
SELECT * FROM EmprestimosOptions