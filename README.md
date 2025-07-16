# My Game List – API ASP.NET

API para cadastro de jogos e avaliações de usuários.  
**URL pública:** `https://minhagamelist.azurewebsites.net/`  
**URL local (dev):** `http://localhost:5122/`

## Rotas Disponíveis

| Rota                              | Método | Descrição                                                       | Permissão                 |
| --------------------------------- | ------ | --------------------------------------------------------------- | ------------------------- |
| `/api/auth/register`              | POST   | Registra um novo usuário                                        | Nenhuma                   |
| `/api/auth/login`                 | POST   | Retorna um token JWT para autenticação                          | Nenhuma                   |
| `/api/jogos`                      | GET    | Retorna todos os jogos com gêneros, imagem de capa e nota média | Nenhuma                   |
| `/api/jogos/{id}`                 | GET    | Retorna jogo com todas as informações detalhadas e avaliações.  | Nenhuma                   |
| `/api/jogos/{id}`                 | DELETE | Remove jogo do banco de dados                                   | Nenhuma                   |
| `/api/generos`                    | GET    | Retorna todos os gêneros de jogos cadastrados.                  | Nenhuma                   |
| `/api/generos?id={id}&id={id}...` | GET    | Retorna todos os gêneros de jogos filtrados na consulta.        | Nenhuma                   |
| `/api/sugerirjogo`                | POST   | Usuário sugere um jogo para aprovação                           | `user`                    |
| `/api/sugerirjogo`                | GET    | Retorna uma lista de todos as sugestões de jogos                | `admin`                   |
| `/api/sugerirjogo/aprovar/{id}`   | POST   | Admin aprova jogo previamente sugerido                          | `admin`                   |
| `/api/sugerirjogo/reprovar/{id}`  | DELETE | Admin reprova jogo previamente sugerido                         | `admin`                   |
| `/api/avaliacoes/jogo`            | POST   | Cria ou atualiza avaliação de um jogo                           | `user`                    |
| `/api/avaliacoes/jogo/{id}`       | GET    | Retorna todas as avaliações de um jogo                          | Nenhuma                   |
| `/api/avaliacoes/usuario/{id}`    | GET    | Retorna todas as avaliações feitas por um usuário               | Nenhuma                   |
| `/api/avaliacoes/{id}`            | DELETE | Remove avaliação do banco                                       | `user` (autor) ou `admin` |

---

### ⚠️ Observação

Para rodar o projeto localmente, é necessário configurar os seguintes parâmetros no arquivo de variáveis de ambiente (`.env`) ou no `appsettings.json`:

- **String de conexão do banco de dados (Azure SQL)**
- **String de conexão do Blob Storage da Azure**
- **Nome do container de blobs onde as imagens são armazenadas**
- **Chave secreta (JWT_SECRET) usada para geração e validação de tokens JWT**

Sem esses dados, a aplicação **não funcionará corretamente**.
