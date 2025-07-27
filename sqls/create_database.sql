DROP SCHEMA IF EXISTS "minha-game-list" CASCADE;
CREATE SCHEMA "minha-game-list";

create table "minha-game-list".Roles(
	Id serial primary key not null,
	Nome text not null
);
create table "minha-game-list".Usuarios(
	Id serial primary key not null,
	Login text not null unique,
	SenhaHash text not null,
	RoleId integer not null,
	foreign key (RoleId) references "minha-game-list".Roles(Id)
);
create table "minha-game-list".Jogos(
	Id serial primary key not null,
	Nome text not null unique
);
create table "minha-game-list".Generos(
	Id serial primary key not null,
	Nome text not null unique
);
create table "minha-game-list".JogosGeneros(
	JogoId integer not null,
	GeneroId integer not null,
	primary key (JogoId, GeneroId),
	foreign key (JogoId) references "minha-game-list".Jogos(Id) on delete cascade,
	foreign key (GeneroId) references "minha-game-list".Generos(Id) on delete cascade
);
create table "minha-game-list".TiposImagem(
	Id serial primary key not null,
	Nome text not null unique
);
create table "minha-game-list".Imagens(
	Id serial primary key not null,
	JogoId integer not null,
	TipoId integer not null,
	Url text not null,
	foreign key (JogoId) references "minha-game-list".Jogos(Id) on delete cascade,
	foreign key (TipoId) references "minha-game-list".TiposImagem(Id) on delete cascade
);
create table "minha-game-list".Avaliacoes(
	Id serial primary key not null,
	JogoId integer not null,
	UsuarioId integer not null,
	Nota integer not null check (Nota >= 0 and Nota <= 10),
	Opiniao text,
	DataCriacao timestamptz not null default now(),
	foreign key (JogoId) references "minha-game-list".Jogos(Id) on delete cascade,
	foreign key (UsuarioId) references "minha-game-list".Usuarios(Id) on delete cascade
);
create table "minha-game-list".SugestoesJogo(
	Id serial primary key not null,
	UsuarioId integer not null,
	JogoAprovadoId integer,
	Nome text not null,
	Aprovado boolean not null,
	DataSugestao timestamptz not null default now(),
	foreign key (UsuarioId) references "minha-game-list".Usuarios(Id) on delete cascade,
	foreign key (JogoAprovadoId) references "minha-game-list".Jogos(Id) on delete cascade
);
create table "minha-game-list".SugestoesJogoGeneros(
	SugestaoJogoId integer not null,
	GeneroId integer not null,
	primary key (SugestaoJogoId, GeneroId),
	foreign key (SugestaoJogoId) references "minha-game-list".SugestoesJogo(Id) on delete cascade,
	foreign key (GeneroId) references "minha-game-list".Generos(Id) on delete cascade
);
create table "minha-game-list".SugestoesImagem(
	Id serial primary key not null,
	SugestaoJogoId integer not null,
	TipoId integer not null,
	Url text not null,
	foreign key (SugestaoJogoId) references "minha-game-list".SugestoesJogo(Id) on delete cascade,
	foreign key (TipoId) references "minha-game-list".TiposImagem(Id) on delete cascade
);
