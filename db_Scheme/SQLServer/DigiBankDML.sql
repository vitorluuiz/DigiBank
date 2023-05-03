USE DIGIBANK

INSERT INTO Condicoes(Condicao)
VALUES ('À pagar'),('Pago')
SELECT * FROM Condicoes
GO

INSERT INTO Usuarios(NomeCompleto, Apelido, CPF, Telefone, Email, Senha, DigiPoints, RendaFixa, Saldo)
VALUES 
('Administrador', 'ADM', '99999999909', '08004040100', 'SAC@digibank.com', '$2a$12$ir1TB.1e463lUaqed9jFWOoIqkTmRLonakE64C6uOPJtV74B9F42a', 100000, 15000, 100000),
('José Aparecido da Cunha', 'José', '14527324509', '11972482459', 'Jose.cunha@gmail.com', '$2a$12$01BMdz4IPYSQJPvvwTmT4..yOMEGe77AdDorwxURDK7jLZ1JfCm.G', 0, 2500, 0),
('Marcio Fernandes Lima', 'Marcio', '48461712308', '11962472896', 'Marcio.Lima@gmail.com', '$2a$12$0Ay12NT365RKa4XPat9uneWRiJMLucKVa9sP1AwZpTLHZanA28vtK', 0, 4000, 0)
SELECT * FROM Usuarios
GO

INSERT INTO TipoInvestimentos(TipoInvestimento)
VALUES ('Instituição financeira'),('Criptomoeda'),('Fundo Imobiliário de Comércios'),('Fundo Imobiliário residencial'),('Tecnologia digital'),('Construtora')
SELECT * FROM TipoInvestimentos
GO

INSERT INTO InvestimentoOptions(idTipoInvestimento, Nome, Descricao, CodeId, Img, Dividendos, IndiceConfiabilidade, IndiceDividendos, IndiceValorizacao, ValorInicial)
VALUES (1,'DigiBank Corporation', 'Compre um pedaçinho de nós. Participe do núcleo de investidores apoiadores da DigiBank, fazendo parte de nosso futuro a partir do capital aberto mundial', 'DIGIBK','ImagemFalsa.exe', 2.45, 4.5, 2, 3.3, 32.14)
SELECT * FROM InvestimentoOptions
GO

INSERT INTO Investimento(idUsuario, idInvestimentoOption, QntCotas, DataAquisicao, DepositoInicial)
VALUES (1, 1, 2, '21/09/2022', 60.35)
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