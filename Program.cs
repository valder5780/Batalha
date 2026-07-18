using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.InteropServices.Marshalling;
using Raylib_cs;

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
            /* Comandos basicos de Raylib, abre desenha e fecha uma janela
            Raylib.InitWindow(800, 480, "Batalha");
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Orange);
            Raylib.DrawText("Hello my friend", 300, 230, 20, Color.Black);
            Raylib.EndDrawing();
            Raylib.CloseWindow();
            */

            Console.WriteLine("gerando mapa");
            Mapa mapa1 = new Mapa();
            mapa1.definir_espaço(50, 50);
            mapa1.gerar_mapa();
            mapa1.mostrar_mapa();

            /*Console.WriteLine("me diga seu nome");
            nome = Console.ReadLine();

            Console.Write(nome + " ");
                
            */
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
                Console.WriteLine("gerando mapa");
                Mapa mapa1 = new Mapa();
                mapa1.definir_espaço(15, 15);
                mapa1.gerar_mapa(4, 5, 8, 3);
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
        public void definir_espaço(int xAxis = 25, int yAxis = 25)
        {
            mapaComp = new int[xAxis][];
            for(int i = 0; i < mapaComp.Length; i++)
            {
                mapaComp[i] = new int[yAxis];  
            }
            //sai tudo com zero nos valores indefinidos


        }
        
        //legenda: aleat_chuncks = numero de chunks, esp_alea = e a chance de um zero virar 1 e de 1/esp_alea,
        // connects = quantas pontes entre as chuncks vao ser colocadas, defCon_chance = a chance usar o algoritimo 1/defCon_chance
        //juntCam_chance = a chance de conectar espacos adjacentes (101 -> 111)
        public void gerar_mapa(int aleat_chuncks = 6, int esp_alea = 8
        , int conects = 8, int defCon_chance = 1, int juntCam_chance = 2)
        { 

            Random rnd = new();
            int[] mChuncks_x = new int[aleat_chuncks]; //guarda a casa do centro que representa a chuncks matriz (5x5) eixo x
            int[] mChuncks_y = new int[aleat_chuncks]; //guarda a casa do centro que representa a chuncks matriz (5x5) eixo y
            int chuncksRegist = 0; //marca quantas chuncks 5x5 estao registradas

            int[] microChuncks_x = new int[aleat_chuncks]; //guarda a casa que representar a matriz menor
            int[] microChuncks_y = new int[aleat_chuncks]; //guarda a casa que representar a matriz menor
            int microChuncksRegist = 0;

            //colocar para adicionar usando chuncksCon.Add() o index de todas as chuncks que ja tiverem pelo menos uma conexao com outra, essa lista no final 
            //tem que ter todos os index de todas as chuncks
            List<int> chuncksCon = new List<int>(); 


            //usei 2+ e o menos 3 para ele nao pegar cordenadas de inicio muito perto dos limites da array
            //cria x chunks base 5x5 ------------------------------------------------------------------------------------------------------------------------
            for(int ci = 0; ci < aleat_chuncks; ci++)
            {
                //Obs: ele nao pode ficar abaixo de reSortNumbers, por que se nao ele vai ficar resetando para zero e nao vai funcionar
                int repeatCount = 0; 

                
                reSortNumbers:

                int start_x = 2 + rnd.Next(mapaComp.Length - 4); 
                int start_y = 2 + rnd.Next(mapaComp[0].Length - 4);
                
                
                
                int TrySpacedCount = 0; 
                
                

                //usei chunck_x para contar mas e so pq chunck x e y tem o mesmo tamanho e principio
                //essa parte serve para ver se nao tem outra chunck na mesma posicao que vai construir essa nova chunck (checa pela area 5x5 da chunck ja existente)
                for(int valid = 0; valid < chuncksRegist; valid++)
                {
                    //caa -> x ou x <-aac                                                 e      caa -> x ou x <- aac
                    if((mChuncks_x[valid] + 5 < start_x || mChuncks_x[valid] - 5 > start_x) || (mChuncks_y[valid] + 5 < start_y || mChuncks_y[valid] - 5 > start_y))
                    {
                        TrySpacedCount += 1;
                    }
                } 

                if(TrySpacedCount == chuncksRegist)
                {
                    for(int i = 0; i < 5; i++)
                    {
                        for(int j = 0; j < 5; j++)
                        {
                            mapaComp[start_x - 2 + i][start_y - 2 + j] = 1;
                        }
                    }
                }
                else
                {

                    repeatCount += 1;

                    if(repeatCount >= 500)
                    {
                        Console.WriteLine("excesso de repetiçao, muitas poucas coordenadas sobraram para colocar chuncks");
                        goto finish_chuncking; // ele sai dessa etapa, deve estar no final das chaves do for()
                    }

                    //recomeça desde o inicio a funçao
                    Console.Write("tentativa_deu_errada");
                    Console.WriteLine(repeatCount + " " + start_x + " " + start_y);
                    
                    goto reSortNumbers;
                }                      
                       
                //apos criar as chuncks ele sobe as cordenadas centrais delas
                mChuncks_x[ci] = start_x;
                mChuncks_y[ci] = start_y;
                chuncksRegist += 1;

            } 
            finish_chuncking:

            Console.WriteLine("primeira etapa concluida");
            mostrar_mapa();
            
            //criar x chuncks extras(SalasExt) YxY (tamanho definido como aleatorio)
            //OBS: a chunck grande define a base de coordenada no meio, essa define no canto superior esquerdo ´\ 
            
            for(int ci = 0; ci < aleat_chuncks; ci++)
            {
                int tam_ChunckX = rnd.Next(3) + 2; //vai de 2 ate 4 (2+2) o tamanho
                int tam_ChunckY = rnd.Next(3) + 2;// vai de 2 ate 4 o tamanho
                int SalasExt_cordX = rnd.Next(mapaComp.Length - 1);
                int SalasExt_cordY = rnd.Next(mapaComp[0].Length);
                microChuncks_x[ci] = SalasExt_cordX;
                microChuncks_y[ci] = SalasExt_cordY;
                microChuncksRegist += 1;

                for(int i = 0; i < tam_ChunckX  && SalasExt_cordX + i < mapaComp.Length -1; i++) //a segunda parte o && serve para ele nao chegar na parede e dar exception out of range
                {
                    for(int j = 0; j < tam_ChunckY && SalasExt_cordY + j < mapaComp[0].Length -1; j++)
                    {
                        mapaComp[SalasExt_cordX + i][SalasExt_cordY + j] = 1;
                    }
                }
            }
            Console.WriteLine("segunda etapa concluida");
            mostrar_mapa();

            //conectando as chuncks-----------------------------------------------------------------------------------------------------
            for(int i = 0; i < conects; i++)
            {
                int pCon = rnd.Next(aleat_chuncks);
                int sCon = rnd.Next(aleat_chuncks);
                if(pCon == sCon && sCon != mChuncks_x.Length - 1 ) //a segunda parte e para ter certeza que se
                {                                                 //posso adicionar mais um em sCon sem dar erro
                    sCon += 1;
                }
                if(pCon == sCon) // nao especifiquei mais pois se ele nao passar no de cima ele entra nesse
                {
                    sCon -= 1;
                }
                for(int j = 0; j < Math.Abs(mChuncks_x[pCon] - mChuncks_x[sCon]); j++)
                {
                    if(mChuncks_x[pCon] > mChuncks_x[sCon])
                    {

                        mapaComp[mChuncks_x[pCon] - j][mChuncks_y[pCon]] = 1;
                        
                    }
                    else
                    {
                        mapaComp[mChuncks_x[pCon] + j][mChuncks_y[pCon]] = 1;
                    }

                }
                for(int j = 0; j < Math.Abs(mChuncks_y[pCon] - mChuncks_y[sCon]); j++)
                {
                    if(mChuncks_y[pCon] > mChuncks_y[sCon])
                    {
                        mapaComp[mChuncks_x[sCon]][mChuncks_y[sCon] + j] = 1;
                    }
                    else
                    {
                        mapaComp[mChuncks_x[sCon]][mChuncks_y[sCon] - j] = 1;
                    }
                }
                
                mapaComp[mChuncks_x[sCon]][mChuncks_y[pCon]] = 1;

            }
            Console.WriteLine("terceira etapa concluida");
            mostrar_mapa();
            
            //resolvi tirar a aleatoriedade de 1 no mapa *****
            /*//coloca 1 aleatoriamente no mapa ---------------------------------------------------------------------------------------------------------
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
            Console.WriteLine("quarta etapa concluida");
            mostrar_mapa();*/

            //revisar se esse algoritimo funciona ******
            //algoritmo para criar caminhos, se no caminho nao tiver conexoes ele conecta ele
            for(int i = 0; i < mapaComp.Length; i++)
            {
                for(int j = 0; j < mapaComp[i].Length; j++)
                {
                    int mod = 0;                           
                    if(mapaComp[i][j] == 1)
                    {
                        
                        for(int xAdd = -1; xAdd < 2; xAdd++)
                        {
                            for(int yAdd = -1; yAdd < 2; yAdd++)
                            {
                                //aqui eu comecei a criar um sistema para o algoritmo _____________________________________________________________________________________________________
                                
                                int xt = i + xAdd;
                                int yt = j + yAdd;
                                if(xt <= mapaComp.Length -1 && xt != -1 && 
                                yt <= mapaComp.Length -1  && yt != -1)  
                                {
                                        
                                    if(mapaComp[xt][yt] == 0)
                                    {
                                        //0 nos cantos += 1; 0 nas rotas adjacentes  += 4
                                        mod += 1;
                                        if(xt == 0 || yt == 0 ) 
                                        {
                                            mod += 3; 
                                        }
                                    }
                                }
                                    

                            }

                        }
                        int a = rnd.Next(defCon_chance);
                            
                        if(mod >= 16 && a != 0) // coloquei 10 para medir ramos principais, lembrando que mod mede o numero de paredes
                        {
                            int aleat_add = rnd.Next(4);
                                

                            restartSwitch:

                            switch(aleat_add)
                            {
                                case 0:
                                    if(i != mapaComp.Length - 1 && mapaComp[i+ 1][j] == 0)
                                    {
                                        mapaComp[i+ 1][j] = 1;
                                        break;
                                    }
                                    aleat_add = rnd.Next(4);
                                    

                                        
                                    goto restartSwitch;

                                case 1:
                                    if(i != 0 && mapaComp[i-1][j] == 0)
                                    {
                                        mapaComp[i-1][j] = 1;
                                        break;
                                    }
                                    aleat_add = rnd.Next(4);
                                        

                                    goto restartSwitch;

                                case 2:
                                        
                                    if(j != mapaComp[i].Length - 1 && mapaComp[i][j+1] == 0)
                                    {
                                        mapaComp[i][j+1] = 1;
                                        break;
                                    }

                                    aleat_add = rnd.Next(4);

                                    goto restartSwitch;

                                case 3:
                                    if(j != 0 && mapaComp[i][j-1] == 0)
                                    {
                                        mapaComp[i][j-1] = 1;
                                        break;
                                    }
                                    aleat_add = rnd.Next(4);

                                    goto restartSwitch;

                            }
                                
                                 //Revisar essa parte ****************************************

                        } // mod tem que ser maior 


                    }
                        //essa parte junta os caminhos que estao de frente um para o outro
                    if(mapaComp[i][j] == 0 && rnd.Next(juntCam_chance) == 0)
                    {
                        try
                        {
                            if(mapaComp[i+1][j] == 1 && mapaComp[i-1][j] == 1)
                            {
                                mapaComp[i][j] = 1;
                            }
                        }
                        catch{}
                        try
                        {
                            if(mapaComp[i][j+1] == 1 && mapaComp[i][j-1] == 1)
                            {
                                mapaComp[i][j] = 1;
                            }
                        }
                        catch{}

                    }
                }
            }
            Console.WriteLine("quinta etapa concluida");
            Console.WriteLine(" ");
            mostrar_mapa();

            //talvez possa ser retirado ja que vou tirar os 1 aleatorios do mapa ****
            //limpa os 1 que estao perdidos no mapa
            for(int i = 0; i < mapaComp.Length; i++)
            {
                for(int j = 0; j < mapaComp.Length; j++)
                {
                    if(mapaComp[i][j] == 1)
                    {


                        if((i == 0 || mapaComp[i-1][j] == 0) && (i == mapaComp.Length -1 || mapaComp[i+1][j] == 0) && 
                        (j == 0 || mapaComp[i][j-1] == 0) && (j == mapaComp[i].Length -1 || mapaComp[i][j+1] == 0))
                        {
                            mapaComp[i][j] = 0;   
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



