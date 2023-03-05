USE DIGIBANK

INSERT INTO Condicoes(Condicao)
VALUES ('À pagar'),('Pago')
GO

INSERT INTO Usuarios(NomeCompleto, Apelido, CPF, Telefone, Email, Senha, DigiPoints, RendaFixa, Saldo)
VALUES 
('Administrador', 'ADM', '99999999909', '08004040100', 'SAC@digibank.com', '$2a$12$ir1TB.1e463lUaqed9jFWOoIqkTmRLonakE64C6uOPJtV74B9F42a', 9999999, 15000.00, 9999999),
('José Aparecido da Cunha', 'José', '14527324509', '11972482459', 'Jose.cunha@gmail.com', '$2a$12$01BMdz4IPYSQJPvvwTmT4..yOMEGe77AdDorwxURDK7jLZ1JfCm.G', 0, 2500.67, 0),
('Marcio Fernandes Lima', 'Marcio', '48461712308', '11962472896', 'Marcio.Lima@gmail.com', '$2a$12$0Ay12NT365RKa4XPat9uneWRiJMLucKVa9sP1AwZpTLHZanA28vtK', 0, 4000.23, 0)
GO

INSERT INTO AcoesOptions(Nome, Descricao, Codigo, DividendoAnual, CotasDisponiveis, IndiceConfiabilidade, IndiceDividendos, IndiceValorizacao)
VALUES ('DigiBank Corporation', 'Compre um pedaçinho de nós. Participe do núcleo de investidores apoiadores da DigiBank, fazendo parte de nosso futuro a partir do capital aberto mundial', 'DIGIBK', 0.013, 5000, 4, 3, 3)
GO

INSERT INTO Acoes(idUsuario, idAcoesOptions, QntCotas, DataAquisicao, ValorInicial)
VALUES (1, 1, 250, '21/09/2022', 100)

INSERT INTO EmprestimosOptions(Valor, TaxaJuros, RendaMinima, PrazoEstimado)
VALUES (1000, 0.35, 0, 30),
(2000, 0.3, 0, 60),
(5000, 0.20, 800, 90),
(10000, 0.17, 2000, 210),
(20000, 0.15, 3500, 365),
(50000, 0.10, 4500, 730),
(75000, 0.10, 5000, 1095)

INSERT INTO TiposFundos(TipoFundo)
VALUES ('Fundo Imobiliário'), ('Previdência privada')

INSERT INTO FundosOptions(idTipoFundo, IndiceConfiabilidade, IndiceDividendos, IndiceValorizacao, TaxaJuros)
VALUES (1, 3, 2, 2, 0.013),
(2, 4, 3, 3, 0.01)