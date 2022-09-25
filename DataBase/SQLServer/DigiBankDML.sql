USE DIGIBANK

INSERT INTO Condicoes(Condicao)
VALUES ('À pagar'),('Pago')
GO

INSERT INTO Usuarios(NomeCompleto, Apelido, CPF, Telefone, Email, Senha, PontosVantagem, RendaFixa)
VALUES 
('Administrador', 'ADM', '99999999909', '11981162489', 'ADM@gmail.com', '123456789', 0, 15000.00),
('José Aparecido da Cunha', 'José', '14527324509', '11972482459', 'Jose.cunha@gmail.com', 'josecunha123', 0, 2500.67),
('Marcio Fernandes Lima', 'Marcio', '48461712308', '11962472896', 'Marcio.Lima@gmail.com', 'marciolima123', 0, 4000.23)
GO

INSERT INTO AcoesOptions(Nome, Descricao, Codigo, Dividendos, CotasDisponiveis, IndiceConfiabilidade, IndiceDividendos, IndiceValorizacao)
VALUES ('DigiBank Corporation', 'Compre um pedaçinho de nós. Participe do núcleo de investidores apoiadores da DigiBank, fazendo parte de nosso futuro a partir do capital aberto mundial', 'DIGIBK', 0.013, 5000, 4, 3, 3)
GO

INSERT INTO Acoes(idUsuario, idAcoesOptions, QntCotas, DataAquisicao, ValorInicial)
VALUES (1, 1, 250, '21/09/2022', 100)

INSERT INTO EmprestimosOptions(Valor, TaxaJuros, RendaMinima)
VALUES (1000, 0.35, 0),
(2000, 0.3, 0),
(5000, 0.20, 800),
(10000, 0.17, 2000),
(20000, 0.15, 3500),
(50000, 0.10, 4500),
(75000, 0.10, 5000)

INSERT INTO TiposFundos(TipoFundo)
VALUES ('Fundo Imobiliário'), ('Previdência privada')

INSERT INTO FundosOptions(idTipoFundo, IndiceConfiabilidade, IndiceDividendos, IndiceValorizacao, Dividendos, TaxaJuros)
VALUES (1, 3, 2, 2, 0.013, 0.013),
(2, 4, 3, 3, 0.01, 0.01)


