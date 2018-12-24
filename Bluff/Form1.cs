using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;
using System.IO;

namespace Bluff
{
    public partial class Form1 : Form
    {
        public Player player1;
        public Player player2;
        public Player player3;
        public Player player4;
        List<Image> cards;
        Stack<Image> deckCards;
        List<Image> flushCards=new List<Image>();
        List<SelectCard> plrCards;
        PlayerDetail playerForm;
        public Player PlayerPlaying;
        public Player lastPlayer;
        List<string> arangedList;
        int numOfCards;
        string cardName;
        int pagenumber = 1;

        static string temppath = Application.ExecutablePath;
        static string _path = temppath.Replace("\\bin\\Debug\\Bluff.EXE","");

        //SoundPlayer player = new SoundPlayer(Path.Combine(_path, ConfigurationManager.AppSettings["Sound"]));
        static Stream strr = Bluff.Properties.Resources.sound;
        SoundPlayer player = new SoundPlayer(strr);

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcFonts);
        FontFamily ff1, ff2, ff3, ff4;
        Font f1B, f2C, f3Ci, f4O;
        public Form1()
        {
            player.Play();
            InitializeComponent();
            
        }

        private void loadFonts()
        {
            byte[] fontArray1 = Bluff.Properties.Resources.BionicKidSimple;
            byte[] fontArray2 = Bluff.Properties.Resources.CrashLandingBB;
            //byte[] fontArray3 = Bluff.Properties.Resources.CrashLandingBB_ital;
            byte[] fontArray4 = Bluff.Properties.Resources.OLDENGL;

            int fontDArray1 = Bluff.Properties.Resources.BionicKidSimple.Length;
            int fontDArray2 = Bluff.Properties.Resources.CrashLandingBB.Length;
            //int fontDArray3 = Bluff.Properties.Resources.CrashLandingBB_ital.Length;
            int fontDArray4 = Bluff.Properties.Resources.OLDENGL.Length;

            IntPtr f1_ptrData = Marshal.AllocCoTaskMem(fontDArray1);
            IntPtr f2_ptrData = Marshal.AllocCoTaskMem(fontDArray2);
            //IntPtr f3_ptrData = Marshal.AllocCoTaskMem(fontDArray3);
            IntPtr f4_ptrData = Marshal.AllocCoTaskMem(fontDArray4);

            Marshal.Copy(fontArray1, 0, f1_ptrData, fontDArray1);
            Marshal.Copy(fontArray2, 0, f2_ptrData, fontDArray2);
            //Marshal.Copy(fontArray3, 0, f3_ptrData, fontDArray3);
            Marshal.Copy(fontArray4, 0, f4_ptrData, fontDArray4);

            uint cf1 = 0;
            uint cf2 = 0;
            //uint cf3 = 0;
            uint cf4 = 0;
            AddFontMemResourceEx(f1_ptrData, (uint)fontArray1.Length, IntPtr.Zero, ref cf1);
            AddFontMemResourceEx(f2_ptrData, (uint)fontArray2.Length, IntPtr.Zero, ref cf2);
            //AddFontMemResourceEx(f3_ptrData, (uint)fontArray3.Length, IntPtr.Zero, ref cf3);
            AddFontMemResourceEx(f4_ptrData, (uint)fontArray4.Length, IntPtr.Zero, ref cf4);

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddMemoryFont(f1_ptrData, fontDArray1);
            Marshal.FreeCoTaskMem(f1_ptrData);
            pfc.AddMemoryFont(f2_ptrData, fontDArray2);
            Marshal.FreeCoTaskMem(f2_ptrData);
            //pfc.AddMemoryFont(f3_ptrData, fontDArray3);
            //Marshal.FreeCoTaskMem(f3_ptrData);
            pfc.AddMemoryFont(f4_ptrData, fontDArray4);
            Marshal.FreeCoTaskMem(f4_ptrData);

            ff1 = pfc.Families[0];
            ff2 = pfc.Families[1];
            //ff3 = pfc.Families[2];
            ff4 = pfc.Families[2];

            f1B = new Font(ff1, 15f);
            f2C = new Font(ff2,  15f);
            //f3Ci = new Font(ff3, 15f);
            f4O = new Font(ff4, 15f);
        }

        private void AllocFont(FontFamily ff, Font f, Control c, FontStyle style, float size)
        {
            FontStyle fontStyle = style;
            c.Font = new Font(ff, size, fontStyle, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        public void SetFonts()
        {
            loadFonts();

            AllocFont(ff2, f2C, p1_Name, FontStyle.Regular, 27.75F);
            AllocFont(ff2, f2C, p2_Name, FontStyle.Regular, 27.75F);
            AllocFont(ff2, f2C, p3_Name, FontStyle.Regular, 27.75F);
            AllocFont(ff2, f2C, p4_Name, FontStyle.Regular, 27.75F);

            AllocFont(ff1, f1B, p1_Turn, FontStyle.Regular, 14.25F);
            AllocFont(ff1, f1B, p2_Turn, FontStyle.Regular, 14.25F);
            AllocFont(ff1, f1B, p3_Turn, FontStyle.Regular, 14.25F);
            AllocFont(ff1, f1B, p4_Turn, FontStyle.Regular, 14.25F);

            AllocFont(ff2, f2C, p1_ncards, FontStyle.Regular, 24F);
            AllocFont(ff2, f2C, p2_ncards, FontStyle.Regular, 24F);
            AllocFont(ff2, f2C, p3_ncards, FontStyle.Regular, 24F);
            AllocFont(ff2, f2C, p4_ncards, FontStyle.Regular, 24F);

            AllocFont(ff1, f1B, p1_Bluff, FontStyle.Regular, 9F);
            AllocFont(ff1, f1B, p2_Bluff, FontStyle.Regular, 9F);
            AllocFont(ff1, f1B, p3_Bluff, FontStyle.Regular, 9F);
            AllocFont(ff1, f1B, p4_Bluff, FontStyle.Regular, 9F);

            AllocFont(ff2, f2C, desk_Count, FontStyle.Regular, 27.75F);

            AllocFont(ff1, f1B, p1_Passb, FontStyle.Regular, 9.749999F);
            AllocFont(ff1, f1B, p1_Playb, FontStyle.Regular, 9.749999F);
            AllocFont(ff1, f1B, p2_Passb, FontStyle.Regular, 9.749999F);
            AllocFont(ff1, f1B, p2_Playb, FontStyle.Regular, 9.749999F);
            AllocFont(ff1, f1B, p3_Passb, FontStyle.Regular, 9.749999F);
            AllocFont(ff1, f1B, p3_Playb, FontStyle.Regular, 9.749999F);
            AllocFont(ff1, f1B, p4_Passb, FontStyle.Regular, 9.749999F);
            AllocFont(ff1, f1B, p4_Playb, FontStyle.Regular, 9.749999F);

            AllocFont(ff4, f4O, b_start, FontStyle.Regular, 24F);
            AllocFont(ff2, f2C, lb_boardCall, FontStyle.Regular, 26.25F);
            AllocFont(ff2, f2C, lb_firstcall, FontStyle.Regular, 26.25F);

            AllocFont(ff1, f1B, b_Bluff, FontStyle.Regular, 15.75F);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Intialize Fonts 
            player.Play();
            SetFonts();
            player1 = new Player(p1_Name, p1_ncards, p1_Bluff, p1_Turn, p1_Passb,p1_Playb, new List<Image>(), panel_p1, 1);
            player2 = new Player(p2_Name, p2_ncards, p2_Bluff, p2_Turn, p2_Passb, p2_Playb, new List<Image>(), panel_p2, 2);
            player3 = new Player(p3_Name, p3_ncards, p3_Bluff, p3_Turn, p3_Passb, p3_Playb, new List<Image>(), panel_p3, 3);
            player4 = new Player(p4_Name, p4_ncards, p4_Bluff, p4_Turn, p4_Passb, p4_Playb, new List<Image>(), panel_p4, 4);
            PlayerPlaying = player1;
            lastPlayer = PlayerPlaying;
            PlayerPlaying.Pass.Enabled = false;
            if(lastPlayer.playerName==PlayerPlaying.playerName){
                b_Bluff.Enabled = false;
            }
            addCards();
            Shuffle(cards);
            divideCards();
            deckCards = new Stack<Image>();

            //playerForm = new PlayerDetail();
            //addsCards();
        } 
        public void Play(Player p) 
        {
            desk_Count.Text ="Deck Count : " + deckCards.Count.ToString();
            ImageSelect();

            if (winCheck()) {
                player1.myPanel.Visible = false;
                player1.turnOn.Visible = false;
                player1.cardsLeft.Text = "Cards Left: " + player1.myCards.Count.ToString();
                player2.myPanel.Visible = false;
                player2.turnOn.Visible = false;
                player2.cardsLeft.Text = "Cards Left: " + player2.myCards.Count.ToString();
                player3.myPanel.Visible = false;
                player3.turnOn.Visible = false;
                player3.cardsLeft.Text = "Cards Left: " + player3.myCards.Count.ToString();
                player4.myPanel.Visible = false;
                player4.turnOn.Visible = false;
                player4.cardsLeft.Text = "Cards Left: " + player4.myCards.Count.ToString();
                lb_firstcall.Text = "Original Call: " + cardName;
                if (winCheck())
                {
                    if (!p.win)
                    {
                        p.myPanel.Visible = true;
                        p.turnOn.Visible = true;
                        p.cardsLeft.Text = "Cards Left: " + p.myCards.Count.ToString();
                        PlayerPlaying = p;
                        
                        if (lastPlayer.playerName == PlayerPlaying.playerName)
                        {
                            b_Bluff.Enabled = false;
                        }
                        else {
                            b_Bluff.Enabled = true;
                        }
                    }
                    else
                    {
                        if (p.playerNumber == 1)
                        {
                            Play(player2);
                        }
                        else if (p.playerNumber == 2)
                        {
                            Play(player3);
                        }
                        else if (p.playerNumber == 3)
                        {
                            Play(player4);
                        }
                        else
                        {
                            Play(player1);
                        }
                    }
                }
                else {
                    MessageBox.Show("Game Is Over...!");
                }
                
            }
        }       
        public bool winCheck() {
            if(!player1.win&&!player2.win&&!player3.win){
                return true;
            }
            else if (!player1.win && !player2.win && !player4.win)
            {
                return true;
            }
            else if (!player1.win && !player3.win && !player4.win)
            {
                return true;
            }
            if (!player2.win && !player3.win && !player4.win)
            {
                return true;
            }
            
            return false;
        }    
        public void addsCards() {
            plrCards.Add(new SelectCard(playerForm.pb_1, playerForm.cb_1));
            plrCards.Add(new SelectCard(playerForm.pb_2, playerForm.cb_2));
            plrCards.Add(new SelectCard(playerForm.pb_3, playerForm.cb_3));
            plrCards.Add(new SelectCard(playerForm.pb_4, playerForm.cb_4));
            plrCards.Add(new SelectCard(playerForm.pb_5, playerForm.cb_5));
            plrCards.Add(new SelectCard(playerForm.pb_6, playerForm.cb_6));
            plrCards.Add(new SelectCard(playerForm.pb_7, playerForm.cb_7));
            plrCards.Add(new SelectCard(playerForm.pb_8, playerForm.cb_8));
            plrCards.Add(new SelectCard(playerForm.pb_9, playerForm.cb_9));
            plrCards.Add(new SelectCard(playerForm.pb_10, playerForm.cb_10));
            plrCards.Add(new SelectCard(playerForm.pb_11, playerForm.cb_11));
            plrCards.Add(new SelectCard(playerForm.pb_12, playerForm.cb_12));
            plrCards.Add(new SelectCard(playerForm.pb_13, playerForm.cb_13));
            plrCards.Add(new SelectCard(playerForm.pb_14, playerForm.cb_14));
        }
        public void divideCards() {
            for (int i = 0; i < 52; ) {
                player1.myCards.Add(cards[i++]);
                player2.myCards.Add(cards[i++]);
                player3.myCards.Add(cards[i++]);
                player4.myCards.Add(cards[i++]);
            }
        }
        public void Shuffle(List<Image> list) {
            Random rng = new Random();
            int n = list.Count;
            while(n>1){
                n--;
                int k = rng.Next(n+1);
                Image value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public void addCards() {
            cards = new List<Image>();
            cards.Add(global::Bluff.Properties.Resources._2C);
            cards[0].Tag = "2";
            cards.Add(global::Bluff.Properties.Resources._2D);
            cards[1].Tag = "2";
            cards.Add(global::Bluff.Properties.Resources._2H);
            cards[2].Tag = "2";
            cards.Add(global::Bluff.Properties.Resources._2S);
            cards[3].Tag = "2";
            cards.Add(global::Bluff.Properties.Resources._3C);
            cards[4].Tag = "3";
            cards.Add(global::Bluff.Properties.Resources._3D);
            cards[5].Tag = "3";
            cards.Add(global::Bluff.Properties.Resources._3H);
            cards[6].Tag = "3";
            cards.Add(global::Bluff.Properties.Resources._3S);
            cards[7].Tag = "3";
            cards.Add(global::Bluff.Properties.Resources._4C);
            cards[8].Tag = "4";
            cards.Add(global::Bluff.Properties.Resources._4D);
            cards[9].Tag = "4";
            cards.Add(global::Bluff.Properties.Resources._4H);
            cards[10].Tag = "4";
            cards.Add(global::Bluff.Properties.Resources._4S);
            cards[11].Tag = "4";
            cards.Add(global::Bluff.Properties.Resources._5C);
            cards[12].Tag = "5";
            cards.Add(global::Bluff.Properties.Resources._5D);
            cards[13].Tag = "5";
            cards.Add(global::Bluff.Properties.Resources._5H);
            cards[14].Tag = "5";
            cards.Add(global::Bluff.Properties.Resources._5S);
            cards[15].Tag = "5";
            cards.Add(global::Bluff.Properties.Resources._6C);
            cards[16].Tag = "6";
            cards.Add(global::Bluff.Properties.Resources._6D);
            cards[17].Tag = "6";
            cards.Add(global::Bluff.Properties.Resources._6H);
            cards[18].Tag = "6";
            cards.Add(global::Bluff.Properties.Resources._6S);
            cards[19].Tag = "6";
            cards.Add(global::Bluff.Properties.Resources._7C);
            cards[20].Tag = "7";
            cards.Add(global::Bluff.Properties.Resources._7D);
            cards[21].Tag = "7";
            cards.Add(global::Bluff.Properties.Resources._7H);
            cards[22].Tag = "7";
            cards.Add(global::Bluff.Properties.Resources._7S);
            cards[23].Tag = "7";
            cards.Add(global::Bluff.Properties.Resources._8C);
            cards[24].Tag = "8";
            cards.Add(global::Bluff.Properties.Resources._8D);
            cards[25].Tag = "8";
            cards.Add(global::Bluff.Properties.Resources._8H);
            cards[26].Tag = "8";
            cards.Add(global::Bluff.Properties.Resources._8S);
            cards[27].Tag = "8";
            cards.Add(global::Bluff.Properties.Resources._9C);
            cards[28].Tag = "9";
            cards.Add(global::Bluff.Properties.Resources._9D);
            cards[29].Tag = "9";
            cards.Add(global::Bluff.Properties.Resources._9H);
            cards[30].Tag = "9";
            cards.Add(global::Bluff.Properties.Resources._9S);
            cards[31].Tag = "9";
            cards.Add(global::Bluff.Properties.Resources._10C);
            cards[32].Tag = "10";
            cards.Add(global::Bluff.Properties.Resources._10D);
            cards[33].Tag = "10";
            cards.Add(global::Bluff.Properties.Resources._10H);
            cards[34].Tag = "10";
            cards.Add(global::Bluff.Properties.Resources._10S);
            cards[35].Tag = "10";
            cards.Add(global::Bluff.Properties.Resources.JC);
            cards[36].Tag = "J";
            cards.Add(global::Bluff.Properties.Resources.JD);
            cards[37].Tag = "J";
            cards.Add(global::Bluff.Properties.Resources.JH);
            cards[38].Tag = "J";
            cards.Add(global::Bluff.Properties.Resources.JS);
            cards[39].Tag = "J";
            cards.Add(global::Bluff.Properties.Resources.QC);
            cards[40].Tag = "Q";
            cards.Add(global::Bluff.Properties.Resources.QD);
            cards[41].Tag = "Q";
            cards.Add(global::Bluff.Properties.Resources.QH);
            cards[42].Tag = "Q";
            cards.Add(global::Bluff.Properties.Resources.QS);
            cards[43].Tag = "Q";
            cards.Add(global::Bluff.Properties.Resources.KC);
            cards[44].Tag = "K";
            cards.Add(global::Bluff.Properties.Resources.KD);
            cards[45].Tag = "K";
            cards.Add(global::Bluff.Properties.Resources.KH);
            cards[46].Tag = "K";
            cards.Add(global::Bluff.Properties.Resources.KS);
            cards[47].Tag = "K";
            cards.Add(global::Bluff.Properties.Resources.AC);
            cards[48].Tag = "A";
            cards.Add(global::Bluff.Properties.Resources.AD);
            cards[49].Tag = "A";
            cards.Add(global::Bluff.Properties.Resources.AH);
            cards[50].Tag = "A";
            cards.Add(global::Bluff.Properties.Resources.AS);
            cards[51].Tag = "A";
        }
        public void sortCards(Player p) {
            arangedList = new List<string>();
            List<Image> newSortedCards = new List<Image>();
            addArangeList();
            foreach (string s in arangedList)
            {
                foreach (Image img in p.myCards)
                {
                    if (img.Tag.ToString().Equals(s))
                    {
                        newSortedCards.Add(img);
                    }
                }
            }
            p.myCards.Clear();
            p.myCards.AddRange(newSortedCards);
        }
        public void addArangeList() {
            arangedList.Add("A");
            arangedList.Add("K");
            arangedList.Add("Q");
            arangedList.Add("J");
            arangedList.Add("10");
            arangedList.Add("9");
            arangedList.Add("8");
            arangedList.Add("7");
            arangedList.Add("6");
            arangedList.Add("5");
            arangedList.Add("4");
            arangedList.Add("3");
            arangedList.Add("2");
        }
        public void eachPlayerUpdate(Player player) {
            this.Hide();
            playerForm = new PlayerDetail();
            playerForm.FormClosed += playerForm_FormClosed;
            playerForm.b_call.Click += b_call_Click;
            playerForm.b_Next.Click +=b_Next_Click;
            playerForm.b_prev.Click += b_prev_Click;
            playerForm.pName_lb.Text = player.playerName.Text;
            PlayerPlaying = player;
            plrCards = new List<SelectCard>(14);
            addsCards();
            sortCards(PlayerPlaying);
            playerForm.Show();
            int i = 0;
            
            foreach (Image s in player.myCards)
            {
                if (i < 14)
                {
                    plrCards[i].myPB.Image = s;
                    plrCards[i++].myCB.Enabled = true;
                }
                else {
                    playerForm.b_Next.Visible = true;
                }
            }
            if (PlayerPlaying.Pass.Enabled == true)
            {
                playerForm.cmb_call.Items.Clear();
                playerForm.cmb_call.Items.Add("more");
            }
            else
            {
                playerForm.cmb_call.Items.Clear();
                playerForm.cmb_call.Items.AddRange(arangedList.ToArray());
            }
        }

        private void b_prev_Click(object sender, EventArgs e)
        {
            pagenumber--;
            playerForm.b_Next.Visible = true;
            foreach (SelectCard s in plrCards)
            {
                s.myPB.Image = null;
                s.myCB.Enabled = false;
            }
            int j = 0;
            for (int i = pagenumber*14; i < PlayerPlaying.myCards.Count; i++)
            {
                if(j<14){
                plrCards[j].myPB.Image = PlayerPlaying.myCards[i];
                plrCards[j++].myCB.Enabled = true;
                }
            }
            if(pagenumber==0){
                playerForm.b_prev.Visible = false;
                pagenumber = 1;
            }
            
        }
        
        private void b_Next_Click(object sender, EventArgs e)
        {
            
            playerForm.b_prev.Visible = true;
            foreach(SelectCard s in plrCards){
                s.myPB.Image = null;
                s.myCB.Enabled = false;
            }
            
            int j = 0;
            for (int i = pagenumber * 14; i < PlayerPlaying.myCards.Count; i++ ) {
                if(j<14){
                plrCards[j].myPB.Image = PlayerPlaying.myCards[i];
                plrCards[j++].myCB.Enabled = true;
                }
            }

            if (PlayerPlaying.myCards.Count < (pagenumber + 1) * 14)
            {
                playerForm.b_Next.Visible = false;
            }
            else {
                pagenumber++;
            }
            
        }

        private void p1_Playb_Click(object sender, EventArgs e)
        {
            eachPlayerUpdate(player1);
        }

        private void p2_Playb_Click(object sender, EventArgs e)
        {
            eachPlayerUpdate(player2);
        }

        private void p3_Playb_Click(object sender, EventArgs e)
        {
            eachPlayerUpdate(player3);
        }

        private void p4_Playb_Click(object sender, EventArgs e)
        {
            eachPlayerUpdate(player4);
        }

        private void playerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void b_start_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_p1.Text) &&
                !string.IsNullOrWhiteSpace(tb_p2.Text) &&
                !string.IsNullOrWhiteSpace(tb_p3.Text) &&
                !string.IsNullOrWhiteSpace(tb_p4.Text))
            {
                player1.playerName.Text = tb_p1.Text;
                player2.playerName.Text = tb_p2.Text;
                player3.playerName.Text = tb_p3.Text;
                player4.playerName.Text = tb_p4.Text;
                panel_start.Hide();
            }
            else {
                MessageBox.Show("You must enter all 4 players names...!");
            }
        }

        private void b_call_Click(object sender, EventArgs e)
        {
            int n = 0;
            if (playerForm.cmb_call.SelectedItem != null)
            {
                lastPlayer = PlayerPlaying;
                foreach (SelectCard s in plrCards)
                {
                    if (s.myCB.Checked)
                    {
                        n++;
                    }
                }
                if (n < 1 || n > 4)
                {
                    MessageBox.Show("You Must Select Atleast 1 and Atmost 4...!");
                    return;
                }
                if (PlayerPlaying.Pass.Enabled == false && n < 2)
                {
                    MessageBox.Show("AtFirst Turn there must be more than 1 Card...!");
                    return;
                }
                
                foreach (SelectCard s in plrCards)
                {
                    if (s.myCB.Checked)
                    {
                        deckCards.Push(s.myPB.Image);

                        if (playerForm.pName_lb.Text.Equals(player1.playerName.Text))
                        {
                            player1.myCards.Remove(s.myPB.Image);
                            player1.cardsLeft.Text = "Cards Left:  " + player1.myCards.Count.ToString();
                            Play(player2);
                        }
                        else if (playerForm.pName_lb.Text.Equals(player2.playerName.Text))
                        {
                            player2.myCards.Remove(s.myPB.Image);
                            player2.cardsLeft.Text = "Cards Left:  " + player2.myCards.Count.ToString();
                            Play(player3);
                        }
                        else if (playerForm.pName_lb.Text.Equals(player3.playerName.Text))
                        {
                            player3.myCards.Remove(s.myPB.Image);
                            player3.cardsLeft.Text = "Cards Left:  " + player3.myCards.Count.ToString();
                            Play(player4);
                        }
                        else
                        {
                            player4.myCards.Remove(s.myPB.Image);
                            player4.cardsLeft.Text = "Cards Left:  " + player4.myCards.Count.ToString();
                            Play(player1);
                        }
                    }

                }

                this.lb_boardCall.Text = playerForm.pName_lb.Text + " Calls: " + n.ToString() + ", " + playerForm.cmb_call.SelectedItem.ToString() + " Cards ";
                numOfCards = n;
                if (!playerForm.cmb_call.SelectedItem.ToString().Equals("more"))
                {
                    cardName = playerForm.cmb_call.SelectedItem.ToString();
                }

                this.Show();
                playerForm.Dispose();
                lastPlayer.Pass.Enabled = true;
                player.Play();

            }
            else
            {
                MessageBox.Show("Select Some area in combo box...!");
            }
        }

        private void p1_Passb_Click(object sender, EventArgs e)
        {
            if (PlayerPlaying.playerNumber == lastPlayer.playerNumber)
            {
                flushCards.AddRange(deckCards);
                Play(PlayerPlaying);
                PlayerPlaying.Pass.Enabled = false;
                deckCards.Clear();
                desk_Count.Text = "Deck Count : " + deckCards.Count.ToString();
                ImageSelect();
            }
            else
            {
                if (((Button)sender).Name.Equals("p1_Passb"))
                {
                    Play(player2);
                    
                }
                else if (((Button)sender).Name.Equals("p2_Passb"))
                {
                    Play(player3);
                    
                }
                else if (((Button)sender).Name.Equals("p3_Passb"))
                {
                    Play(player4);
                    
                }
                else
                {
                    Play(player1);
                    
                }
            }
            
        }

        private void b_Bluff_Click(object sender, EventArgs e)
        {
            bool jhoot = false;
            for (int i = 0; i < numOfCards; i++ ) {
                if (!deckCards.ElementAt(i).Tag.ToString().Equals(cardName)) {
                    jhoot = true;
                }
            }
         
            if (jhoot)
            {
                
                lastPlayer.myCards.AddRange(deckCards);
                deckCards.Clear();
                Play(PlayerPlaying);
                lastPlayer.bluffCount++;
                lastPlayer.bluff.Text = "Bluff: " + lastPlayer.bluffCount.ToString();
                PlayerPlaying.Pass.Enabled = false;
                b_Bluff.Enabled = false;
            }
            else {
                PlayerPlaying.myCards.AddRange(deckCards);
                deckCards.Clear();
                Play(lastPlayer);
                lastPlayer.Pass.Enabled = false;
                
            }
            player.Play();
        }

        public void ImageSelect()
        {
            if (deckCards.Count == 0) 
            {
                deck_Panel.BackgroundImage = null;
            }
            else if (deckCards.Count == 1) 
            {
                deck_Panel.BackgroundImage = global::Bluff.Properties.Resources.one;
            }
            else if (deckCards.Count == 2)
            {
                deck_Panel.BackgroundImage = global::Bluff.Properties.Resources.two;
            }
            
            else
            {
                deck_Panel.BackgroundImage = global::Bluff.Properties.Resources.three;
            }
        }

    }
    public class SelectCard {
        public PictureBox myPB;
        public CheckBox myCB;
        public SelectCard():this(null,null) { }
        public SelectCard(PictureBox myPB, CheckBox myCB) {
            this.myCB = myCB;
            this.myPB = myPB;
        }
    }
    public class Player {
        public Label playerName;
        public Label cardsLeft;
        public Label bluff;
        public int bluffCount = 0;
        public Label turnOn;
        public Button Pass;
        public Button Play;
        public bool isturn = false;
        public int playerNumber;
        public Panel myPanel;
        public List<Image> myCards;
        public Player():this(null,null,null,null,null,null,null,null, 0) { }
        public Player(Label playerName, Label cardsLeft, Label bluff, Label turnOn,Button Pass,Button Play, List<Image> myCards, Panel myPanel, int playerNumber) {
            this.playerName = playerName;
            this.cardsLeft = cardsLeft;
            this.bluff = bluff;
            this.turnOn = turnOn;
            this.Pass = Pass;
            this.Play = Play;
            this.myCards = myCards;
            this.myPanel = myPanel;
            this.playerNumber = playerNumber;
        }
        public bool win {
            get { if(myCards.Count==0){
                return true;
            }
            return false;
            }
        }
    }
    
}

