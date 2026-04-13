using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace CrudAlunos
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString =
            "Server=localhost;Database=Escola;User ID=root;Password=fiap;AllowPublicKeyRetrieval=True;SslMode=Disabled;";

        public MainWindow()
        {
            InitializeComponent();
            CarregarAlunos();
        }

        private void CarregarAlunos()
        {
            try
            {
                var alunos = new List<Aluno>();

                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, Nome, Idade FROM Alunos ORDER BY Id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alunos.Add(new Aluno
                            {
                                Id = reader.GetInt32("Id"),
                                Nome = reader.GetString("Nome"),
                                Idade = reader.GetInt32("Idade")
                            });
                        }
                    }
                }

                dgAlunos.ItemsSource = alunos;
                MostrarStatus($"✅ {alunos.Count} aluno(s) carregado(s).", "#27AE60");
            }
            catch (Exception ex)
            {
                MostrarStatus($"❌ Erro ao listar: {ex.Message}", "#E74C3C");
            }
        }

        private void BtnListar_Click(object sender, RoutedEventArgs e)
            => CarregarAlunos();

        private void BtnInserir_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarNomeIdade()) return;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Alunos (Nome, Idade) VALUES (@Nome, @Idade)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                        cmd.Parameters.AddWithValue("@Idade", int.Parse(txtIdade.Text));
                        cmd.ExecuteNonQuery();
                    }
                }

                MostrarStatus($"✅ Aluno '{txtNome.Text.Trim()}' inserido!", "#27AE60");
                LimparCampos();
                CarregarAlunos();
            }
            catch (Exception ex)
            {
                MostrarStatus($"❌ Erro ao inserir: {ex.Message}", "#E74C3C");
            }
        }

        private void BtnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarId()) return;
            if (!ValidarNomeIdade()) return;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Alunos SET Nome = @Nome, Idade = @Idade WHERE Id = @Id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                        cmd.Parameters.AddWithValue("@Idade", int.Parse(txtIdade.Text));
                        cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));

                        int linhas = cmd.ExecuteNonQuery();

                        if (linhas == 0)
                            MostrarStatus($"⚠️ Nenhum aluno com ID {txtId.Text}.", "#E67E22");
                        else
                        {
                            MostrarStatus($"✅ Aluno ID {txtId.Text} atualizado!", "#27AE60");
                            LimparCampos();
                            CarregarAlunos();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarStatus($"❌ Erro ao atualizar: {ex.Message}", "#E74C3C");
            }
        }

        private void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarId()) return;

            var resposta = MessageBox.Show(
                $"Deseja remover o aluno com ID {txtId.Text}?",
                "Confirmar Remoção",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resposta != MessageBoxResult.Yes) return;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Alunos WHERE Id = @Id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));

                        int linhas = cmd.ExecuteNonQuery();

                        if (linhas == 0)
                            MostrarStatus($"⚠️ Nenhum aluno com ID {txtId.Text}.", "#E67E22");
                        else
                        {
                            MostrarStatus($"✅ Aluno ID {txtId.Text} removido!", "#27AE60");
                            LimparCampos();
                            CarregarAlunos();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarStatus($"❌ Erro ao remover: {ex.Message}", "#E74C3C");
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarId()) return;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT Id, Nome, Idade FROM Alunos WHERE Id = @Id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("Id");
                                string nome = reader.GetString("Nome");
                                int idade = reader.GetInt32("Idade");

                                txtNome.Text = nome;
                                txtIdade.Text = idade.ToString();

                                dgAlunos.ItemsSource = new List<Aluno>
                                {
                                    new Aluno { Id = id, Nome = nome, Idade = idade }
                                };

                                MostrarStatus($"✅ Encontrado: {nome}, {idade} anos.", "#27AE60");
                            }
                            else
                            {
                                MostrarStatus($"⚠️ Nenhum aluno encontrado com ID {txtId.Text}.", "#E67E22");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarStatus($"❌ Erro ao buscar: {ex.Message}", "#E74C3C");
            }
        }

        private void DgAlunos_SelectionChanged(object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgAlunos.SelectedItem is Aluno aluno)
            {
                txtId.Text = aluno.Id.ToString();
                txtNome.Text = aluno.Nome;
                txtIdade.Text = aluno.Idade.ToString();
            }
        }

        private void BtnLimpar_Click(object sender, RoutedEventArgs e)
            => LimparCampos();

        private void BtnSair_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show("Deseja sair?", "Sair",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void LimparCampos()
        {
            txtId.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtIdade.Text = string.Empty;
        }

        private void MostrarStatus(string mensagem, string corHex = "#333333")
        {
            txtStatus.Text = mensagem;
            txtStatus.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(corHex));
        }

        private bool ValidarId()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out _))
            {
                MostrarStatus("⚠️ Informe um ID válido.", "#E67E22");
                txtId.Focus();
                return false;
            }
            return true;
        }

        private bool ValidarNomeIdade()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MostrarStatus("⚠️ O Nome não pode estar vazio.", "#E67E22");
                txtNome.Focus();
                return false;
            }
            if (!int.TryParse(txtIdade.Text, out int idade) || idade <= 0 || idade > 120)
            {
                MostrarStatus("⚠️ Informe uma Idade válida (1–120).", "#E67E22");
                txtIdade.Focus();
                return false;
            }
            return true;
        }
    }
}