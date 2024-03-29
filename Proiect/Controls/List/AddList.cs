﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Proiect.Controls.List
{
    public partial class AddList : UserControl
    {
        Button addListButton;
        public AddList(Button button)
        {
            addListButton = button;
            InitializeComponent();

            addListButton.Visible = false;
            this.textBox1.Select();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string nume = this.textBox1.Text;

            Form1 parent = (Form1)this.ParentForm;

            if (nume != "")
            {
                OleDbConnection connection = new OleDbConnection(Form1.Provider);
                OleDbCommand command = new OleDbCommand();

                command.Connection = connection;

                try
                {
                    connection.Open();
                    command.Transaction = connection.BeginTransaction();

                    command.CommandText = "INSERT INTO liste (nume, id_autor) VALUES (@nume, @autor)";

                    command.Parameters.Add("nume", OleDbType.Char).Value = nume;
                    command.Parameters.Add("autor", OleDbType.Integer).Value = parent.utilizator.Id;


                    command.ExecuteNonQuery();
                    command.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    command.Transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                    
                    //refresh the layout panel

                    this.addListButton.Visible = true;
                    this.Dispose();

                    parent.LoadLists();
                }

            }
            else
            {
                errorProvider1.SetError(textBox1, "Va rugam sa setati un nume!");
            }
        }

        private void AddList_Leave(object sender, EventArgs e)
        {
            this.addListButton.Visible = true;
            this.Dispose();
        }
    }
}
