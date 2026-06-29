using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;

namespace Batalha
{
    
    class Classe
    {
        static string nome;
        static int[]  status = new int[4];
        static int sttIndex = 0;
        static string statsName;
        static int RI;
        static int pontos = 10;

        static void Main()
        {
            Console.WriteLine("me diga seu nome");
            nome = Console.ReadLine();

            Console.Write(nome + " ");
            Distrib_Pontos();
        }

        static void Distrib_Pontos()
        {
            
            Console.WriteLine ("você tem " + pontos + " pontos, distribua eles em forca, defesa, habilidade e velocidade");
            Console.WriteLine("primeiro diga quantos pontos vai distribuir em força");
            statsName = "força";
            RI = int.Parse(Console.ReadLine());
            pontosTeste();
            Console.WriteLine("agora defina quantos pontos vão para sua defesa");
            statsName = "defesa";
            RI = int.Parse(Console.ReadLine());
            pontosTeste();
            Console.WriteLine("agora diga quantos pontos vai colocar em habilidade");
            statsName = "habilidade";
            RI = int.Parse(Console.ReadLine());
            pontosTeste();
            Console.WriteLine("agora por fim diga quantos pontos vai por em velocidade");
            statsName = "velocidade";
            RI = int.Parse(Console.ReadLine());
            pontosTeste();
            sttIndex = 0;
            printStatus();
            Console.WriteLine("agora vamos comecar");
            jogo();
        }
        static void jogo()
        {
            Console.WriteLine("oque voce gostaria de fazer? 1- mapa, 2- ver seus status");
            RI = int.Parse(Console.ReadLine());
            if(RI != 1 && RI != 2)
            {
                valor_outRange(1,2);
            }
            if(RI == 1)
            {
                Mapa mapa1 = new Mapa();
                mapa1.definir_espaço(20, 25);
                mapa1.gerar_mapa(5, 5);
                mapa1.mostrar_mapa();
            }
            if(RI == 2)
            {
                printStatus();
            }

        }

        static void Derrota()
        {

        }

        static void valor_outRange(int R_inic, int R_fin)
        {
            if(RI < R_inic || RI > R_fin )
            {
                Console.WriteLine("coloque um valor valido");
                RI = int.Parse(Console.ReadLine());
                valor_outRange(R_inic, R_fin);
            }
        }
        static void printStatus()
        {
            Console.WriteLine("seus status:");
            Console.WriteLine("For: " + status[0]);
            Console.WriteLine("Def: " + status[1]);
            Console.WriteLine("Hab: " + status[2]);
            Console.WriteLine("Vel: " + status[3]);
        }

        static void pontosTeste()
        {
            if(RI > pontos)
            {

                Console.WriteLine("voce nao tem essa quantia de pontos, coloque uma quantia menor");
                RI = int.Parse(Console.ReadLine());

                pontosTeste();
            }
            else
            {
                Console.WriteLine("otimo, sua " + statsName + " é " + RI);
                status[sttIndex] += RI; 
                pontos -= RI;
                sttIndex += 1;
                RI = 0;

            }
            
            
        }
        



    }
    class inimigos
    {
        string nome;
        int defesa;
        int forca;
        int velocidade;
        int habilidade;
        int[] ataques;

        

    
    }

    public class Mapa
    {
        public int[][] mapaComp;
        public int a = 2;
        public void definir_espaço(int xAxis = 50, int yAxis = 50)
        {
            mapaComp = new int[xAxis][];
            for(int i = 0; i < mapaComp.Length; i++)
            {
                mapaComp[i] = new int[yAxis];  
            }


        }
        public void gerar_mapa(int aleat_chuncks = 10, int esp_alea = 20)
        {
            Random rnd = new();
            int[] valores = {0,1};

            //usei 2+ e o menos 3 para ele nao pegar cordenadas de inicio muito perto dos limites da array
            

            //cria x chunks 5x5
            for(int ci = 0; ci < aleat_chuncks; ci++)
            {
                int start_x = 2 + rnd.Next(mapaComp.Length - 4); 
                int start_y = 2 + rnd.Next(mapaComp[0].Length - 4);

                for(int i = 0; i < 5; i++)
                {
                    for(int j = 0; j < 5; j++)
                    {
                        mapaComp[start_x - 2 + i][start_y - 2 + j] = 1;
                    }
                }      
            }

            //caso for adicionar mais desses pontos, colocar  

            for(int i = 0; i < mapaComp.Length; i++)
            {
                for(int j = 0; j < mapaComp[0].Length; j++) //coloquei zero no index por que o mapa é retangular
                {
                    if(mapaComp[i][j] == 0)
                    {
                        //faz que 1 a cada x valores (definido por esp_alea) possa ser aleatoriamente tornado 1 
                        int n = rnd.Next(esp_alea);
                        if(n == esp_alea -1 && mapaComp[i][j] == 0)
                        {
                            mapaComp[i][j] = 1;
                        }
                           
                    }
                }
            }
            
            
        }

        public void mostrar_mapa()
        {
            if(mapaComp == null)
            {
                Console.WriteLine("nao é possivel mostrar o mapa, ele nao foi gerado");
                return;
            }

            for(int i = 0; i < mapaComp.Length; i++)
            {
                for(int j = 0; j < mapaComp[i].Length; j++)
                {
                    Console.Write(mapaComp[i][j] + " ");
                }
                Console.WriteLine("");
            }
        }
    }


}



