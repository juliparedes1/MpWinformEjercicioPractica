using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace ejemplo_ado_net
{
    public partial class Form1 : Form
    {

        private List<Pokemon> listaPrincipal;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            cargar();
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");

            

        }

        private void cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPrincipal = negocio.listar();
                dgvPokemons.DataSource = listaPrincipal;
                cargarImagen(listaPrincipal[0].UrlImagen);
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPokemons.CurrentRow != null)
            {
                Pokemon seleccionado;
                seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);

            }
        }


        private void cargarImagen(string urlImagen)
        {
            try
            {

                pbPokemon.Load(urlImagen);

            }
            catch (Exception)
            {

                pbPokemon.Load("https://img.freepik.com/vector-gratis/ilustracion-icono-galeria_53876-27002.jpg?size=626&ext=jpg&ga=GA1.1.1413502914.1719878400&semt=ais_user");
            }
        }

        private void btnAgregarPokemon_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            cargar();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLogica_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logico = false)
        {
            PokemonNegocio Negocio = new PokemonNegocio();
            try
            {
                DialogResult respuesta = MessageBox.Show("Esta seguro Que desea eliminar?", "eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    if (logico == true)
                    {
                        Negocio.eliminarLogico(seleccionado);
                    }
                    else
                    {
                        Negocio.eliminarFisico(seleccionado);
                    }
                        cargar();
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
        }

        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltroRapido.Text.ToUpper();
            if (filtro != "")
            {
                listaFiltrada = listaPrincipal.FindAll(x => x.Nombre.ToUpper().Contains(filtro) || x.Tipo.ToString().ToUpper().Contains(filtro) || x.Numero.ToString().Contains(filtro));
            }
            else
            {
                listaFiltrada = listaPrincipal;
            }

            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            cboCriterio.Items.Clear();
            if (opcion == "Número")
            {
                
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Igual a");
                cboCriterio.Items.Add("Menor a");
            }else
            {
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Contiene");
                cboCriterio.Items.Add("Termina con");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            
            try
            {
                if(cboCampo.SelectedItem == null || cboCriterio.SelectedItem == null)
                {
                    throw new Exception() ;
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Por favor Verifique de haber completado todos los campos");
                MessageBox.Show(ex.ToString());
            }
        }
    }


    

}
