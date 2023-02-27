using Firma.Helpers;
using Firma.Models.Entities;
using Firma.Models.EntitiesForView.Kontrahent;
using Firma.Models.EntitiesForView.ZlecenieKompletacji;
using Firma.Models.EntitiesForView.ZlecenieZakupu;
using Firma.ViewModels.KompletacjaViewModel;
using Firma.ViewModels.KontrachentViewModel;
using Firma.ViewModels.ProduktViewModel;
using Firma.ViewModels.RaportyViewModel;
using Firma.ViewModels.ZakupViewModel;
using Firma.ViewResources;
using Firma.Views;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Firma.ViewModels
{
    /// <summary>
    /// bedzie zawierała kolekcję komend, które pojawiają się w menu lewym oraz kolekcję zakładek
    /// </summary>
    public class MainWindowViewModel:BaseViewModel
    {
        #region Konstruktory
        public MainWindowViewModel():base()
        {
            WidocznoscMenuBocznego = "Visible";
            WidocznoscPaskaNarzedzi = "Visible";
            TekstButtonaMenu = "<<";
            TekstMenuPasekBoczny = BaseResources.UkryjMB;
            TekstMenuPasekNarzedzi = BaseResources.UkryjPN;
        }
        #endregion

        #region Komendy menu i paska narzędzi
        public ICommand NowyProduktCommand => new BaseCommand(() => createView(new NowyProduktViewModel()));
        public ICommand NoweZlecenieZakupuCommand => new BaseCommand(() => createView(new NoweZlecenieZakupuViewModel()));
        public ICommand NoweZlecenieKompCommand => new BaseCommand(() => createView(new NoweZlecenieKompViewModel()));
        public ICommand NowyKontrahentCommand => new BaseCommand(() => createView(new NowyKontrahentViewModel()));
        public ICommand OtworzOknoOCommand => new BaseCommand(() => OtworzOknoO());
        public ICommand ProduktyCommand => new BaseCommand(()=>showAllProdukt());
        public ICommand ZleceniaZakupuCommand => new BaseCommand(()=>showAllZZ());
        public ICommand ZleceniaKompletacjiCommand => new BaseCommand(() => showAllZK());
        public ICommand WszyscyKontrahenciCommand => new BaseCommand(()=>showAllKontrahenci());
        public ICommand RaportMonteraCommand => new BaseCommand(()=> createView(new RaportMonteraViewModel()));
        public ICommand RaportZZCommand => new BaseCommand(()=> createView(new RaportZZViewModel()));
        public ICommand RaportCzasuZKCommand => new BaseCommand(() => createView(new SredniCzasZKViewModel()));


        /// <summary>
        /// Komenda podpięta do przycisku w menu górnym. Wywołuje metodę <see cref="ZmienWidocznoscMenuBocznego"/>.
        /// </summary>
        public ICommand ZmienWidocznoscMenuBocznegoCommand { get { return new BaseCommand(() => ZmienWidocznoscMenuBocznego()); } }
        public ICommand ZmienWidocznoscPaskaNarzedziCommand { get { return new BaseCommand(() => ZmienWidocznoscPaskaNarzedzi()); } }




        #endregion

        #region Przyciski w menu z lewej strony


        private ReadOnlyCollection<CommandViewModel> _Commands;//to jest kolekcja komend w menu lewym
        public ReadOnlyCollection<CommandViewModel> Commands 
        {     
            get 
            { 
                if(_Commands == null)//sprawdzam czy przyciski z lewej strony nie zostały zainicjalizowane
                {
                    List<CommandViewModel> cmds = this.CreateCommands();//tworzę listę przycisków za pomocą funkcji CreateCommands
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds); //tę listę przypisuje do ReadOnlyCollection (bo readOnlyCollection można tylko tworzyć, nie można do niej dodawać)
                }
                return _Commands;
            } 
        }
        
        /// <summary>
        /// tu decydujemy jakie przyciski są w lewym menu
        /// </summary>
        /// <returns></returns>
        private List<CommandViewModel> CreateCommands() 
        {
            Messenger.Default.Register<string>(this, Open);
            Messenger.Default.Register<Kontrahenci>(this, OpenKontrahentEdit);
            Messenger.Default.Register<ZleceniaKompletacji>(this, OpenZKEdit);
            Messenger.Default.Register<ZleceniaZakupu>(this, OpenZZEdit);
            Messenger.Default.Register<Produkty>(this, OpenProduktEdit);
            Messenger.Default.Register<AdresForEdit>(this, OpenAdresEdit);
            Messenger.Default.Register<PZZForEdit>(this, OpenPZZEdit);
            Messenger.Default.Register<EditSklZKForAllView>(this, OpenSZKEdit);
            Messenger.Default.Register<EditProdUbZKForAllView>(this, OpenPUZKEdit);
            return new List<CommandViewModel> 
            {
                new CommandViewModel(BaseResources.Produkt, new BaseCommand(()=>createView(new NowyProduktViewModel()))),
                new CommandViewModel(BaseResources.ListaProduktow, new BaseCommand(() => showAllProdukt())),
                new CommandViewModel(BaseResources.ZlecZakup, new BaseCommand(()=>createView(new NoweZlecenieZakupuViewModel()))),
                new CommandViewModel(BaseResources.ZleceniaZakupu, new BaseCommand(() => showAllZZ())),
                new CommandViewModel(BaseResources.ZlecKomp, new BaseCommand(()=>createView(new NoweZlecenieKompViewModel()))),
                new CommandViewModel(BaseResources.ZleceniaKompletacji, new BaseCommand(() => showAllZK())),
                new CommandViewModel(BaseResources.Kontrahent, new BaseCommand(()=>createView(new NowyKontrahentViewModel()))),
                new CommandViewModel(BaseResources.WszyscyKontrahenci, new BaseCommand(()=>showAllKontrahenci())),
            };
        }


        #endregion

        #region Zakładki
        /// <summary>
        /// to jest kolekcja zakładek
        /// </summary>
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if(_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.onWorkspacesChanged;
                }
                return _Workspaces;
            }
        }

        private void onWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.onWorkspaceRequestClose;
            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.onWorkspaceRequestClose;
        }
        private void onWorkspaceRequestClose(object sender, EventArgs e) 
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel; 
            //workspace.Dispos();
            this.Workspaces.Remove(workspace);
        }

        private void Open(string message)
        {
            switch (message)
            {
                case "Dostawcy Show":
                    WszyscyDostawcyViewModel wszyscyDostawcyViewModel = showAllDostawcy();
                    wszyscyDostawcyViewModel.GdzieWyslac = "Produkty";
                    wszyscyDostawcyViewModel.CzyModyfikowac = false;
                    break;
                case "Dostawcy ZZ Show":
                    WszyscyDostawcyViewModel wszyscyDostawcyViewModel1 = showAllDostawcy();
                    wszyscyDostawcyViewModel1.GdzieWyslac = "ZZ";
                    wszyscyDostawcyViewModel1.CzyModyfikowac = false;
                    break;
                case "Kontrahenci ZK Show":
                    WszyscyKontrahenciViewModel wszyscyKontrahenciViewModel = showAllKontrahenci();
                    wszyscyKontrahenciViewModel.GdzieWyslac = "ZK";
                    wszyscyKontrahenciViewModel.CzyModyfikowac = false;
                    break;
                case "Produkty ZK Show":
                    WszystkieProduktyViewModel wszystkieProduktyViewModel = showAllProdukt();
                    wszystkieProduktyViewModel.GdzieWyslac = "ZK";
                    wszystkieProduktyViewModel.CzyModyfikowac = false;
                    break;
                case "Pozycje Zlecenia Zakupu Add":   
                    createView(new NowaPozycjaZZViewModel());
                    break;
                case "Adresy kontrahenta Add":    
                    createView(new NowyAdresViewModel());
                    break;
                case "RodzajTransportu ZZ Show":
                    createView(new NowyRodzajTransportuViewModel());
                    break;
                case "SposobDostawy ZZ Show":
                    createView(new NowySposobDostawyViewModel());
                    break;
                case "SposobPlatnosci ZZ Show":
                    createView(new NowySposobPlatnosciViewModel());
                    break;
                case "StatusZZ ZZ Show":
                    createView(new NowyStatusZZViewModel());
                    break;
                case "TypTransakcji ZZ Show":
                    createView(new NowyTypTransakcjiViewModel());
                    break;
                case "Składniki zlecenia kompletacji Add":
                    createView(new NowySkladnikZKViewModel());
                    break;
                case "Produkty uboczne zlecenia kompletacji Add":
                    createView(new NowyProduktUbocznyZKViewModel());
                    break;
                case "Status ZK Show":
                    createView(new NowyStatusZKViewModel());
                    break;
                case "Monterzy Show":
                    createView(new NowyMonterViewModel());
                    break;
                case "Jednostka Miary Show":
                    createView(new NowaJednostkaMiaryViewModel());
                    break;
                case "Kod Pcn Show":
                    createView(new NowyKodPcnViewModel());
                    break;
                case "Nowy produkt AddShow":
                    createView(new NowyProduktViewModel());
                    break;
                case "Nowe ZK AddShow":
                    createView(new NoweZlecenieKompViewModel());
                    break;
                case "Nowy Kontrahent AddShow":
                    createView(new NowyKontrahentViewModel());
                    break;
                case "Nowe ZZ AddShow":
                    createView(new NoweZlecenieZakupuViewModel());
                    break;
                case "Kontrahenci Raport ZZ Show":
                    WszyscyDostawcyViewModel wszyscyDostawcyViewModel2 = showAllDostawcy();
                    wszyscyDostawcyViewModel2.GdzieWyslac = "RaportZZ";
                    wszyscyDostawcyViewModel2.CzyModyfikowac = false;
                    break;
            }
        }
        private void OpenKontrahentEdit(Kontrahenci kontrahent) => createView(new EditKontrahentViewModel(kontrahent));
        private void OpenZKEdit(ZleceniaKompletacji zk) => createView(new NoweZlecenieKompViewModel(zk));
        private void OpenZZEdit(ZleceniaZakupu zz) => createView(new NoweZlecenieZakupuViewModel(zz));
        private void OpenProduktEdit(Produkty produkt) => createView(new EditProduktViewModel(produkt));
        private void OpenAdresEdit(AdresForEdit adres) => createView(new EditAdresKontrahentViewModel(adres));
        private void OpenPZZEdit(PZZForEdit pzz) => createView(new NowaPozycjaZZViewModel(pzz));
        private void OpenPUZKEdit(EditProdUbZKForAllView obj) => createView(new NowyProduktUbocznyZKViewModel(obj));
        private void OpenSZKEdit(EditSklZKForAllView obj) => createView(new NowySkladnikZKViewModel(obj));

        #endregion

        #region Funkcje pomocnicze

        //to jest funkcja która 
        /// <summary>
        /// za każdym razem tworzy nową zakładkę 
        /// </summary>
        /// <param name="workspace"></param>
        private void createView(WorkspaceViewModel workspace)
        {
            this.Workspaces.Add(workspace);
            this.setActiveWorkspace(workspace);
        }
        /// <summary>
        /// otwiea zakładkę jeśli jest utworzona, jeśli nie tworzy nową
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="workspace_new"></param>
        
        private WszystkieProduktyViewModel showAllProdukt()
        {
            WszystkieProduktyViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is WszystkieProduktyViewModel) as WszystkieProduktyViewModel;
            if (workspace == null)
            {
                //tworzymy nowa zakladke Wszystkie towary
                workspace = new WszystkieProduktyViewModel();
                //i dodajemy ja do kolekcji zakladek
                this.Workspaces.Add(workspace);
            }
            //aktywujemy zakladke
            this.setActiveWorkspace(workspace);
            return(workspace);
        }
        private void showAllZZ()
        {
            ZleceniaZakupuViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is ZleceniaZakupuViewModel) as ZleceniaZakupuViewModel;
            if (workspace == null)
            {
                //tworzymy nowa zakladke Wszystkie towary
                workspace = new ZleceniaZakupuViewModel();
                //i dodajemy ja do kolekcji zakladek
                this.Workspaces.Add(workspace);
            }
            //aktywujemy zakladke
            this.setActiveWorkspace(workspace);
            //return workspace;
        }
        private void showAllZK()
        {
            ZleceniaKompletacjiViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is ZleceniaKompletacjiViewModel) as ZleceniaKompletacjiViewModel;
            if (workspace == null)
            {
                //tworzymy nowa zakladke Wszystkie towary
                workspace = new ZleceniaKompletacjiViewModel();
                //i dodajemy ja do kolekcji zakladek
                this.Workspaces.Add(workspace);
            }
            //aktywujemy zakladke
            this.setActiveWorkspace(workspace);
        }
        private WszyscyKontrahenciViewModel showAllKontrahenci()
        {
            WszyscyKontrahenciViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is WszyscyKontrahenciViewModel) as WszyscyKontrahenciViewModel;
            if (workspace == null)
            {
                //tworzymy nowa zakladke Wszystkie towary
                workspace = new WszyscyKontrahenciViewModel();
                //i dodajemy ja do kolekcji zakladek
                this.Workspaces.Add(workspace);
            }
            //aktywujemy zakladke
            this.setActiveWorkspace(workspace);
           return workspace;
        }

        private WszyscyDostawcyViewModel showAllDostawcy()
        {
            WszyscyDostawcyViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is WszyscyDostawcyViewModel) as WszyscyDostawcyViewModel;
            if (workspace == null)
            {
                //tworzymy nowa zakladke Wszystkie towary
                workspace = new WszyscyDostawcyViewModel();
                //i dodajemy ja do kolekcji zakladek
                this.Workspaces.Add(workspace);
            }
            //aktywujemy zakladke
            this.setActiveWorkspace(workspace);
            return workspace;
        }

        private void setActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null) collectionView.MoveCurrentTo(workspace);
        }

        /// <summary>
        /// Zmienia widoczność menu bocznego używając <see cref="WidocznoscMenuBocznego"/> na widoczne zwinięte.
        /// </summary>
        private void ZmienWidocznoscMenuBocznego()
        {
            WidocznoscMenuBocznego = WidocznoscMenuBocznego == "Visible" ? "Collapsed" : "Visible";
            TekstButtonaMenu = WidocznoscMenuBocznego == "Visible" ? "<<" : ">>";
            TekstMenuPasekBoczny = WidocznoscMenuBocznego == "Visible" ? BaseResources.UkryjMB : BaseResources.PokazMB;
        }
        /// <summary>
        /// Zmienia widoczność paska narzędzi
        /// </summary>
        private void ZmienWidocznoscPaskaNarzedzi()
        {
            WidocznoscPaskaNarzedzi = WidocznoscPaskaNarzedzi == "Visible" ? "Collapsed" : "Visible";
            TekstMenuPasekNarzedzi = WidocznoscPaskaNarzedzi == "Visible" ? BaseResources.UkryjPN : BaseResources.PokazPN;
        }

        private string _WidocznoscMenuBocznego;
        private string _TekstButtonaMenu;
        private string _TekstMenuPasekBoczny;
        private string _TekstMenuPasekNarzedzi;
        private string _WidocznoscPaskaNarzedzi;
        public string WidocznoscMenuBocznego
        {
            get
            {
                return _WidocznoscMenuBocznego;
            }
            set
            {
                if (_WidocznoscMenuBocznego != value)
                {
                    _WidocznoscMenuBocznego = value;
                    OnPropertyChanged(() => WidocznoscMenuBocznego);
                }
            }
        }
        public string WidocznoscPaskaNarzedzi
        {
            get
            {
                return _WidocznoscPaskaNarzedzi;
            }
            set
            {
                if (_WidocznoscPaskaNarzedzi != value)
                {
                    _WidocznoscPaskaNarzedzi = value;
                    OnPropertyChanged(() => WidocznoscPaskaNarzedzi);
                }
            }
        }
        public string TekstButtonaMenu
        {
            get { return _TekstButtonaMenu; }
            set
            {
                if (_TekstButtonaMenu != value)
                {
                    _TekstButtonaMenu = value;
                    OnPropertyChanged(() => TekstButtonaMenu);
                }
            }
        }
        public string TekstMenuPasekBoczny
        {
            get { return _TekstMenuPasekBoczny; }
            set
            {
                if (_TekstMenuPasekBoczny != value)
                {
                    _TekstMenuPasekBoczny = value;
                    OnPropertyChanged(() => TekstMenuPasekBoczny);
                }
            }
        }
        public string TekstMenuPasekNarzedzi
        {
            get { return _TekstMenuPasekNarzedzi; }
            set
            {
                if (_TekstMenuPasekNarzedzi != value)
                {
                    _TekstMenuPasekNarzedzi = value;
                    OnPropertyChanged(() => TekstMenuPasekNarzedzi);
                }
            }
        }

        private AboutWindow oko = null;
        private void OtworzOknoO()
        {
            if (oko != null) oko.Close();
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.DataContext = new MainWindowViewModel();
            oko = aboutWindow;
            aboutWindow.Show();
        }

       

        #endregion
    }
}
