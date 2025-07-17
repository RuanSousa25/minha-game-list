# My Game List – API ASP.NET

API para cadastro de jogos e avaliações de usuários.  
**URL pública:** `https://minhagamelist.azurewebsites.net/`  
**URL local (dev):** `http://localhost:5122/`

## Rotas Disponíveis

| Índice | Rota                               | Método | Descrição                                                       | Permissão                 |
| ------ | ---------------------------------- | ------ | --------------------------------------------------------------- | ------------------------- |
| 1      | `/api/auth/register`               | POST   | Registra um novo usuário                                        | Nenhuma                   |
| 2      | `/api/auth/login`                  | POST   | Retorna um token JWT para autenticação                          | Nenhuma                   |
| 3      | `/api/jogos`                       | GET    | Retorna todos os jogos com gêneros, imagem de capa e nota média | Nenhuma                   |
| 4      | `/api/jogos/{id}`                  | GET    | Retorna jogo com todas as informações detalhadas e avaliações.  | Nenhuma                   |
| 5      | `/api/jogos/{id}`                  | DELETE | Remove jogo do banco de dados                                   | `admin`                   |
| 6      | `/api/generos`                     | GET    | Retorna todos os gêneros de jogos cadastrados.                  | Nenhuma                   |
| 7      | `/api/generos?id={id}&id={id2}...` | GET    | Retorna todos os gêneros de jogos filtrados na consulta.        | Nenhuma                   |
| 8      | `/api/sugerirjogo`                 | POST   | Usuário sugere um jogo para aprovação                           | `user`                    |
| 9      | `/api/sugerirjogo`                 | GET    | Retorna uma lista de todos as sugestões de jogos                | `admin`                   |
| 10     | `/api/sugerirjogo/aprovar/{id}`    | POST   | Admin aprova jogo previamente sugerido                          | `admin`                   |
| 11     | `/api/sugerirjogo/reprovar/{id}`   | DELETE | Admin reprova jogo previamente sugerido                         | `admin`                   |
| 12     | `/api/avaliacoes/jogo`             | POST   | Cria ou atualiza avaliação de um jogo                           | `user`                    |
| 13     | `/api/avaliacoes/jogo/{id}`        | GET    | Retorna todas as avaliações de um jogo                          | Nenhuma                   |
| 14     | `/api/avaliacoes/usuario/{id}`     | GET    | Retorna todas as avaliações feitas por um usuário               | Nenhuma                   |
| 15     | `/api/avaliacoes/{id}`             | DELETE | Remove avaliação do banco                                       | `user` (autor) ou `admin` |

### 1. `POST /api/auth/register`

**Descrição:**  
Registra login e senha do usuário no banco para acesso futuro.

**Requisição:**

- Método: `POST`
- URL: `/api/auth/register`
- Autenticação: Não necessária

**Body (JSON):**

```json
{
  "login": "ruansousa25",
  "senha": "senhamuitoforte"
}
```

**Resposta (201 Created):**

```json
{
  "message": "Usuario Cadastrado"
}
```

**Erros possíveis:**

- `400 Bad Request`: Credenciais inválidas ou ausentes.
- `400 Bad Request`: A senha deve conter, pelo menos, 8 caraceteres, com letras e números.
- `409 Conflict`: Usuário já cadastrado.
- `500 Internal Server Error`: Erro no servidor.

---

### 2. `POST /api/auth/login`

**Descrição:**  
Autentica o usuário e retorna um token JWT.

**Requisição:**

- Método: `POST`
- URL: `/api/auth/login`
- Autenticação: Não necessária

**Body (JSON):**

```json
{
  "login": "ruansousa25",
  "senha": "senhamuitoforte"
}
```

**Resposta (200 OK):**

```json
{
  "message": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibG9naW50ZXN0ZSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiOSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJleHAiOjE3NTI2NzYxMzF9.AXHNwoIn0ZKJCMA9GVBNK5dV6U-ph7OrIdOwFGfDP-I"
}
```

**Erros possíveis:**

- `401 Unauthorized`: Usuário ou senha incorretos.
- `500 Internal Server Error`: Serviço indisponível, tente novamente mais tarde.

---

### 3. `GET /api/jogos`

**Descrição:**  
Retorna todos os jogos cadastrados, com gêneros, imagem de capa e nota média.

**Requisição:**

- Método: `GET`
- URL: `/api/jogos`
- Autenticação: Não necessária

**Resposta (200 OK):**

```json
[
  {
    "id": 1,
    "nome": "Disco Elysium",
    "nota": 10,
    "generos": [
      "RPG",
      "Hack and Slash",
      "Fantasia",
      "Rhythm",
      "Narrativo",
      "Survival Horror"
    ],
    "imagens": [
      "https://mygameliststorage.blob.core.windows.net/jogos-imagens/disco-elysium.jpg"
    ]
  },
  "..."
]
```

**Erros possíveis:**

- `500 Internal Server Error`: Erro no servidor.

---

### 4. `GET /api/jogos/{id}`

**Descrição:**  
Retorna detalhes completos de um jogo, incluindo avaliações.

**Requisição:**

- Método: `GET`
- URL: `/api/jogos/{id}`
- Autenticação: Não necessária

**Resposta (200 OK):**

```json
{
  "id": 30,
  "nome": "Stardew Valley",
  "nota": 0,
  "generos": ["Farm"],
  "imagens": [
    "https://mygameliststorage.blob.core.windows.net/jogos-imagens/35a139c9-19e9-4502-ae85-768ed0c3f7d6.jpg"
  ]
}
```

**Erros possíveis:**

- `404 Not Found`: Jogo não encontrado.
- `500 Internal Server Error`: Erro no servidor.

---

### 5. `DELETE /api/jogos/{id}`

**Descrição:**  
Remove um jogo do banco de dados.

**Requisição:**

- Método: `DELETE`
- URL: `/api/jogos/{id}`
- Autenticação: `admin`

**Resposta (200 OK):**

```json
{
  "message": "Exclusão realizada com sucesso"
}
```

**Erros possíveis:**

- `404 Not Found`: Jogo não encontrado.
- `403 Forbidden`: Sem permissão para deletar.
- `500 Internal Server Error`: Erro no servidor.

---

### 6. `GET /api/generos`

**Descrição:**  
Retorna todos os gêneros cadastrados.

**Requisição:**

- Método: `GET`
- URL: `/api/generos`
- Autenticação: Não necessária

**Resposta (200 OK):**

```json
[
  {
    "id": 1,
    "nome": "Ação"
  },
  {
    "id": 2,
    "nome": "Aventura"
  },
  {
    "id": 3,
    "nome": "RPG"
  },
  "..."
]
```

**Erros possíveis:**

- `500 Internal Server Error`: Erro no servidor.

---

### 7. `GET /api/generos?id={id}&id={id2}...`

**Descrição:**  
Retorna gêneros filtrados pelos IDs informados.

**Requisição:**

- Método: `GET`
- URL: `/api/generos?id=1&id=2`
- Autenticação: Não necessária

**Resposta (200 OK):**

```json
[
  {
    "id": 1,
    "nome": "Ação"
  },
  {
    "id": 2,
    "nome": "Aventura"
  }
]
```

**Erros possíveis:**

- `400 Bad Request`: IDs inválidos.
- `500 Internal Server Error`: Erro no servidor.

---

### 8. `POST /api/sugerirjogo`

**Descrição:**  
Usuário sugere um novo jogo para aprovação.

**Requisição:**

- Método: `POST`
- URL: `/api/sugerirjogo`
- Autenticação: `user`

**Content-Type:** `multipart/form-data`  
**Campos esperados:**

| Campo        | Tipo          | Descrição                    |
| ------------ | ------------- | ---------------------------- |
| `sugestao`   | `text` (JSON) | Informações do jogo sugerido |
| `imagemCapa` | `file`        | Arquivo da imagem de capa    |

_Exemplo para campo sugestao_

```json
{
  "nome": "Your Turn To Die -Death Game By Majority-",
  "generos": [9, 10, 22, 25, 2]
}
```

**Resposta (201 Created):**

```json
{
  "message": "Sugestão inserida com sucesso"
}
```

**Erros possíveis:**

- `400 Bad Request`: Dados inválidos.
- `401 Unauthorized`: Token ausente ou inválido.
- `500 Internal Server Error`: Erro no servidor.

---

### 9. `GET /api/sugerirjogo`

**Descrição:**  
Admin visualiza todas as sugestões de jogos.

**Requisição:**

- Método: `GET`
- URL: `/api/sugerirjogo`
- Autenticação: `admin`

**Resposta (200 OK):**

```json
[
  {
    "id": 10,
    "usuarioId": 2,
    "nome": "Stardew Valley",
    "generos": ["Farm"],
    "imagens": [
      "https://mygameliststorage.blob.core.windows.net/jogos-imagens/35a139c9-19e9-4502-ae85-768ed0c3f7d6.jpg"
    ],
    "dataSugestao": "2025-07-03T00:48:52.043",
    "aprovado": true
  },
  {
    "id": 11,
    "usuarioId": 5,
    "nome": "Your Turn To Die -Death Game By Majority-",
    "generos": [
      "Aventura",
      "Terror",
      "Puzzle",
      "Horror Psicológico",
      "Visual Novel"
    ],
    "imagens": [
      "https://mygameliststorage.blob.core.windows.net/jogos-imagens/76dd6a10-4341-4478-b980-f576db6a76dd.jpg"
    ],
    "dataSugestao": "2025-07-06T23:31:35.443",
    "aprovado": false
  },
  "..."
]
```

**Erros possíveis:**

- `401 Unauthorized`: Não autenticado.
- `403 Forbidden`: Sem permissão.
- `500 Internal Server Error`: Erro no servidor.

---

### 10. `POST /api/sugerirjogo/aprovar/{id}`

**Descrição:**  
Admin aprova uma sugestão de jogo.

**Requisição:**

- Método: `POST`
- URL: `/api/sugerirjogo/aprovar/{id}`
- Autenticação: `admin`

**Resposta (200 OK):**

```json
{
  "id": 30,
  "nome": "Stardew Valley",
  "nota": 0,
  "generos": ["Farm"],
  "imagens": [
    "https://mygameliststorage.blob.core.windows.net/jogos-imagens/35a139c9-19e9-4502-ae85-768ed0c3f7d6.jpg"
  ]
}
```

**Erros possíveis:**

- `404 Not Found`: Sugestão não encontrada.
- `403 Forbidden`: Sem permissão.
- `500 Internal Server Error`: Erro no servidor.

---

### 11. `DELETE /api/sugerirjogo/reprovar/{id}`

**Descrição:**  
Admin reprova uma sugestão de jogo.

**Requisição:**

- Método: `DELETE`
- URL: `/api/sugerirjogo/reprovar/{id}`
- Autenticação: `admin`

**Resposta (200 OK):**

```json
{
  "message": "Sugestão removida com sucesso"
}
```

**Erros possíveis:**

- `404 Not Found`: Sugestão não encontrada.
- `403 Forbidden`: Sem permissão.
- `500 Internal Server Error`: Erro no servidor.

---

### 12. `POST /api/avaliacoes/jogo`

**Descrição:**  
Cria ou atualiza uma avaliação de jogo por um usuário.

**Requisição:**

- Método: `POST`
- URL: `/api/avaliacoes/jogo`
- Autenticação: `user`

**Body (JSON):**

```json
{
  "jogoId": 31,
  "opiniao": "Muito divertido, não consigo parar de jogar!",
  "nota": 10
}
```

**Resposta (201 Created):**

```json
{
  "id": 8,
  "usuarioId": 4,
  "jogoId": 31,
  "nota": 10,
  "opiniao": "Muito divertido, não consigo parar de jogar!",
  "data": "2025-07-16T14:18:17.2493244Z"
}
```

**Erros possíveis:**

- `400 Bad Request`: Dados inválidos.
- `401 Unauthorized`: Token ausente ou inválido.
- `500 Internal Server Error`: Erro no servidor.

---

### 13. `GET /api/avaliacoes/jogo/{id}`

**Descrição:**  
Retorna todas as avaliações de um jogo.

**Requisição:**

- Método: `GET`
- URL: `/api/avaliacoes/jogo/{id}`
- Autenticação: Não necessária

**Resposta (200 OK):**

```json
[
  {
    "id": 1,
    "usuarioId": 1,
    "jogoId": 1,
    "nota": 10,
    "opiniao": "Ótimo jogo! A história me prendeu do início ao fim e me emocionou a cada segundo. Muita satisfação em conhecer uma obra tão intimista como essa, o meu jogo favorito para sempre.",
    "data": "2025-06-29T18:59:39.437"
  },
  "..."
]
```

**Erros possíveis:**

- `404 Not Found`: Jogo não encontrado.
- `500 Internal Server Error`: Erro no servidor.

---

### 14. `GET /api/avaliacoes/usuario/{id}`

**Descrição:**  
Retorna todas as avaliações feitas por um usuário.

**Requisição:**

- Método: `GET`
- URL: `/api/avaliacoes/usuario/{id}`
- Autenticação: Não necessária

**Resposta (200 OK):**

```json
[
  {
    "id": 7,
    "usuarioId": 5,
    "jogoId": 31,
    "nota": 10,
    "opiniao": "Muito divertido, não consigo parar de jogar!",
    "data": "2025-07-06T23:33:44.713"
  }
  "..."
]
```

**Erros possíveis:**

- `404 Not Found`: Usuário não encontrado.
- `500 Internal Server Error`: Erro no servidor.

---

### 15. `DELETE /api/avaliacoes/{id}`

**Descrição:**  
Remove uma avaliação do banco de dados. Somente o autor ou admin pode excluir.

**Requisição:**

- Método: `DELETE`
- URL: `/api/avaliacoes/{id}`
- Autenticação: `user` (autor) ou `admin`

**Resposta (200 OK):**

```json
{
  "message": "Avaliação removida com sucesso"
}
```

**Erros possíveis:**

- `404 Not Found`: Avaliação não encontrada.
- `403 Forbidden`: Sem permissão.
- `500 Internal Server Error`: Erro no servidor.

---

### ⚠️ Observações

Todos os retornos com mensagem de erro seguem a estrutura a seguir:

```json
"message": "erroMessageContent"
```

Para rodar o projeto localmente, é necessário configurar os seguintes parâmetros no arquivo de variáveis de ambiente (`.env`) ou no `appsettings.json`:

- **String de conexão do banco de dados (Azure SQL)**
- **String de conexão do Blob Storage da Azure**
- **Nome do container de blobs onde as imagens são armazenadas**
- **Chave secreta (JWT_SECRET) usada para geração e validação de tokens JWT**

Sem esses dados, a aplicação **não funcionará corretamente**.
