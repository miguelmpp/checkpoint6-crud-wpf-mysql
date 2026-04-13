# checkpoint6-crud-wpf-mysql

> **Checkpoint 6 — Programação em C#**  
> FIAP — Engenharia de Software  
> Aluno: Miguel Parrado | RM: 554007

Sistema CRUD de gerenciamento de alunos desenvolvido em **C# WPF** com conexão ao **MySQL** via **ADO.NET**.

---

## Tecnologias

- C# / WPF (.NET 6+)
- MySQL 8+
- ADO.NET (`MySql.Data 9.x`)
- Visual Studio 2022

---

## Pré-requisitos

- [Visual Studio 2022](https://visualstudio.microsoft.com/) com workload **Desktop Development with .NET**
- [MySQL Server 8+](https://dev.mysql.com/downloads/mysql/) + MySQL Workbench

---

## Como rodar

### 1. Banco de dados

Abra o **MySQL Workbench** e execute:

```sql
CREATE DATABASE IF NOT EXISTS Escola;
USE Escola;

CREATE TABLE IF NOT EXISTS Alunos (
    Id    INT          AUTO_INCREMENT PRIMARY KEY,
    Nome  VARCHAR(100) NOT NULL,
    Idade INT          NOT NULL
);

INSERT INTO Alunos (Nome, Idade) VALUES ('Ana Silva', 22);
INSERT INTO Alunos (Nome, Idade) VALUES ('Carlos Santos', 25);
INSERT INTO Alunos (Nome, Idade) VALUES ('Maria Oliveira', 20);
```

### 2. Clonar e abrir o projeto

```bash
git clone https://github.com/seu-usuario/checkpoint6-crud-wpf-mysql.git
cd checkpoint6-crud-wpf-mysql
```

Abra o arquivo `CrudAlunos.sln` no Visual Studio 2022.

### 3. Instalar dependência MySQL

No **Package Manager Console** (Tools → NuGet Package Manager):

```
Install-Package MySql.Data
```

### 4. Configurar a senha do MySQL

Em `MainWindow.xaml.cs`, linha 13, substitua `SuaSenha`:

```csharp
private const string ConnectionString =
    "Server=localhost;Database=Escola;User ID=root;Password=SuaSenha;AllowPublicKeyRetrieval=True;SslMode=Disabled;";
```

### 5. Rodar

Pressione **F5** ou clique em **Start**.

---

## Funcionalidades

| # | Função | Descrição |
|---|--------|-----------|
| 1 | Inserir Aluno | Adiciona novo aluno ao banco (INSERT) |
| 2 | Listar Alunos | Carrega todos os alunos na tabela (SELECT) |
| 3 | Atualizar Aluno | Edita Nome e Idade de um aluno (UPDATE) |
| 4 | Remover Aluno | Remove aluno com confirmação (DELETE) |
| ⭐ | Buscar por ID | Localiza aluno pelo ID com mensagem de não encontrado |

> **Dica:** Clique em qualquer linha da tabela para preencher os campos automaticamente.

---

## Estrutura do projeto

```
CrudAlunos/
├── Aluno.cs              → Classe modelo
├── MainWindow.xaml       → Interface WPF
├── MainWindow.xaml.cs    → Lógica CRUD com ADO.NET
└── CrudAlunos.csproj
```

---

## Autor

**Miguel Parrado** — RM 554007  
FIAP — Engenharia de Software, 6º Semestre
