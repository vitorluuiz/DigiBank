USE DIGIBANK

INSERT INTO Condicoes(Condicao)
VALUES ('À pagar'),('Pago')
SELECT * FROM Condicoes
GO

INSERT INTO Usuarios(NomeCompleto, Apelido, CPF, Telefone, Email, Senha, DigiPoints, RendaFixa, Saldo)
VALUES 
('Administrador', 'ADM', '99999999909', '08004040100', 'SAC@digibank.com', '$2a$12$ir1TB.1e463lUaqed9jFWOoIqkTmRLonakE64C6uOPJtV74B9F42a', 100000, 15000, 1000000),
('Vitor Luiz de Carvalho', 'Vitor', '12345678909', '1112345678', 'vitor.sesi21@gmail.com', '$2a$12$ir1TB.1e463lUaqed9jFWOoIqkTmRLonakE64C6uOPJtV74B9F42a', 100, 7000, 10000)
SELECT * FROM Usuarios
GO

INSERT INTO TipoInvestimentos(TipoInvestimento)
VALUES ('Poupança'),('Renda Fixa'),('Ações'), ('Fundos de investimentos'), ('Cripto')
SELECT * FROM TipoInvestimentos
GO

INSERT INTO AreaInvestimento(Area)
VALUES ('Poupança'),('CDB'),('Instituição financeira'),('Fundo Imobiliário residencial'),('Criptomoeda'),('NFT')
SELECT * FROM AreaInvestimento
GO

INSERT INTO InvestimentoOptions(idTipoInvestimento, idAreaInvestimento, Colaboradores, Fundacao, Abertura, Sede, Fundador, Nome, Descricao, Sigla, Logo, MainImg, MainColorHex, PercentualDividendos, ValorAcao, Tick, QntCotasTotais)
Values (1, 1, 1,'21/09/2022', '21/09/2022', 'Brasília, Brasil', 'Banco central do Brasil', 'Poupança', 'Invista seu dinheiro na boa e velha poupança Digibank', 'POUPEC','Poupanca.png', 'Poupanca.png', 'C00414', 0.55,0, '06/06/2023', 1),
(2,2, 1,'21/09/2022', '06/06/2023', 'Brasília, Brasil', 'Governo do Brasil', 'CDB', 'Certificado de depósito bancário', 'CDBBRA','CDB.png', 'CDB.png', 'FFFFFF', 1.13, 500, '06/06/2023', 30000),
(3,3,20045,'21/09/2022', '06/06/2023', 'São Paulo, Brasil', 'Everaldo Martins Magalhães', 'DigiBank Corporation', 'Compre um pedaçinho de nós. Participe do núcleo de investidores apoiadores da DigiBank, fazendo parte de nosso futuro a partir do capital aberto mundial', 'DIGIBK','Logo.exe', 'Banner.exe', 'C00414', 1.13, 101, '06/06/2023', 250000000),
(4,4,36046,'21/09/2022', '06/06/2023', 'Florianopoles, Brasil', 'Matheus Leal Palmuti', 'Sammaleur Entinel SC', 'O maior fundo de Investimentos comerciais de Santa Catarina','CSESC','CSESC.png', 'SESC.png', 'FFFFFF', 0.67, 562.5, '06/06/2023', 22000000),
(5,5,1,'21/09/2022', '06/06/2023', 'Brasília, Brasil', 'Banco central do Brasil', 'Criptomoeda do Brasil', 'Moeda virtual do governo do Brasil', 'BRACON','BRACON.png', 'BRACON.png', 'FFFFFF', 0, 32410, '06/06/2023', 100000000)
SELECT * FROM InvestimentoOptions
GO

INSERT INTO Investimento(idUsuario, idInvestimentoOption, QntCotas, DataAquisicao, DepositoInicial)
VALUES (1, 1, 1, '29/09/2022', 30200), (1, 1, 2, '27/12/2022', 20130),
(1, 2, 2, '06/06/2023', 1000), (1, 2, 1, '10/06/2023', 500),
(1, 3, 3, '21/09/2022', 60.35), (1, 3, 2, '25/09/2022', 60.35),
(1, 4, 2, '06/06/2023', 530.4), (1, 4, 2, '16/06/2023', 541.3),
(1, 5, 1, '06/06/2022', 32400), (1, 5, 10, '06/06/2023', 324000)
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