DROP SCHEMA IF EXISTS minha_game_list CASCADE;
CREATE SCHEMA minha_game_list;

CREATE EXTENSION pg_trgm;

SET search_path TO minha_game_list;

CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL
);

CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    login TEXT NOT NULL UNIQUE,
    senha_hash TEXT NOT NULL,
    role_id INTEGER NOT NULL REFERENCES roles(id)
);

CREATE TABLE jogos (
    id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL UNIQUE
);
CREATE TABLE generos (
    id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL UNIQUE
);
CREATE INDEX idx_generos_nome_trgm ON generos USING gin (nome gin_trgm_ops);

CREATE TABLE jogos_generos (
    jogo_id INTEGER NOT NULL,
    genero_id INTEGER NOT NULL,
    PRIMARY KEY (jogo_id, genero_id),
    FOREIGN KEY (jogo_id) REFERENCES jogos(id) ON DELETE CASCADE,
    FOREIGN KEY (genero_id) REFERENCES generos(id) ON DELETE CASCADE
);

CREATE TABLE tipos_imagem (
    id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL
);

CREATE TABLE imagens (
    id SERIAL PRIMARY KEY,
    jogo_id INTEGER NOT NULL,
    tipo_id INTEGER NOT NULL,
    url TEXT NOT NULL,
    FOREIGN KEY (jogo_id) REFERENCES jogos(id) ON DELETE CASCADE,
    FOREIGN KEY (tipo_id) REFERENCES tipos_imagem(id) ON DELETE CASCADE
);

CREATE TABLE avaliacoes (
    id SERIAL PRIMARY KEY,
    jogo_id INTEGER NOT NULL,
    usuario_id INTEGER NOT NULL,
    nota INTEGER NOT NULL CHECK (nota >= 0 AND nota <= 10),
    opiniao TEXT,
    data_criacao TIMESTAMPTZ NOT NULL DEFAULT now(),
    FOREIGN KEY (jogo_id) REFERENCES jogos(id) ON DELETE CASCADE,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);

CREATE TABLE sugestoes_jogo (
    id SERIAL PRIMARY KEY,
    usuario_id INTEGER NOT NULL,
    aprovador_id INTEGER,
    jogo_aprovado_id INTEGER,
    nome TEXT NOT NULL,
    aprovado BOOLEAN NOT NULL,
    data_sugestao TIMESTAMPTZ NOT NULL DEFAULT now(),
    data_aprovacao TIMESTAMPTZ,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (aprovador_id) REFERENCES usuarios(id) ON DELETE SET NULL,
    FOREIGN KEY (jogo_aprovado_id) REFERENCES jogos(id) ON DELETE SET NULL
);

CREATE TABLE sugestoes_jogo_generos (
    sugestao_jogo_id INTEGER NOT NULL,
    genero_id INTEGER NOT NULL,
    PRIMARY KEY (sugestao_jogo_id, genero_id),
    FOREIGN KEY (sugestao_jogo_id) REFERENCES sugestoes_jogo(id) ON DELETE CASCADE,
    FOREIGN KEY (genero_id) REFERENCES generos(id) ON DELETE CASCADE
);

CREATE TABLE sugestoes_imagem (
    id SERIAL PRIMARY KEY,
    sugestao_jogo_id INTEGER NOT NULL,
    tipo_id INTEGER NOT NULL,
    url TEXT NOT NULL,
    FOREIGN KEY (sugestao_jogo_id) REFERENCES sugestoes_jogo(id) ON DELETE CASCADE,
    FOREIGN KEY (tipo_id) REFERENCES tipos_imagem(id) ON DELETE CASCADE
);
