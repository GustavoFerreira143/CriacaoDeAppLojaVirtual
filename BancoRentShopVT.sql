CREATE DATABASE RentShopVT;
USE RentShopVT;

CREATE TABLE Usuarios (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Contato Varchar(19),
    Senha VARCHAR(255) NOT NULL,
    NomeEmpresa VARCHAR(255),
    CNPJ VARCHAR(18),
    CPF VARCHAR(14),
    AutorizadoVenda BIT DEFAULT 0,
    FotoPerfil VARCHAR(255)
);

CREATE TABLE RedesSociais(
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Usuario BIGINT NOT NULL,
    NomeRede Varchar(255) NOT NULL,
    IconeRede Varchar(100) NOT NULL,
    LinkRede Varchar(255) NOT NULL,
    PerfilUserRede Varchar(255) NOT NULL,
    FOREIGN KEY (Usuario) REFERENCES Usuarios(id)
);