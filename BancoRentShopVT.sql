CREATE DATABASE RentShopVT;
USE RentShopVT;

CREATE TABLE Usuarios (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Contato Varchar(19),
    Senha VARCHAR(30) NOT NULL,
    NomeEmpresa VARCHAR(255),
    CNPJ VARCHAR(18),
    CPF VARCHAR(14),
    AutorizadoVenda BIT DEFAULT 0,
    FotoPerfil VARCHAR(255)
);