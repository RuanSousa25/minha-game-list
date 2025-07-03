# My Game List – API ASP.NET

API para cadastro de jogos e avaliações de usuários.  
**URL pública:** `https://minhagamelist.azurewebsites.net/`  
**URL local (dev):** `http://localhost:5122/`

## Rotas Disponíveis

| Rota                                      | Método | Descrição                                                                 | Permissão |
|-------------------------------------------|--------|---------------------------------------------------------------------------|-----------|
| `/api/auth/register`                      | POST   | Registra um novo usuário                                                  | Nenhuma   |
| `/api/auth/login`                         | POST   | Retorna um token JWT para autenticação                                   | Nenhuma   |
| `/api/jogos`                              | GET    | Retorna todos os jogos com gêneros, imagem de capa e nota média          | Nenhuma   |
| `/api/sugerirjogo`                        | POST   | Usuário sugere um jogo para aprovação                                     | `user`    |
| `/api/sugerirjogo/aprovar/{id}`           | POST   | Admin aprova jogo previamente sugerido                                   | `admin`   |
| `/api/avaliacoes/jogo`                    | POST   | Cria ou atualiza avaliação de um jogo                                     | `user`    |
| `/api/avaliacoes/jogo/{id}`               | GET    | Retorna todas as avaliações de um jogo                                   | Nenhuma   |
| `/api/avaliacoes/usuario/{id}`            | GET    | Retorna todas as avaliações feitas por um usuário                        | Nenhuma   |

---

### ⚠️ Observação

Para rodar o projeto localmente, é necessário configurar os seguintes parâmetros no arquivo de variáveis de ambiente (`.env`) ou no `appsettings.json`:

- **String de conexão do banco de dados (Azure SQL)**  
- **String de conexão do Blob Storage da Azure**  
- **Nome do container de blobs onde as imagens são armazenadas**  
- **Chave secreta (JWT_SECRET) usada para geração e validação de tokens JWT**

Sem esses dados, a aplicação **não funcionará corretamente**.
