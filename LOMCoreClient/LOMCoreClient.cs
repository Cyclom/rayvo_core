using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;
using NativeUI;


namespace LOMCoreClient
{
    public class LOMCoreClient : BaseScript
    {

        int vehicle = 0;





        // VARIABLES

        private MenuPool _copMenuPool;

        private MenuPool _carMenuPool;
        private bool isOnDuty = false;


        public String _ccSteamID;

        public static String prefix = "[RayvoGC // Client]";


        //CLIENT SIDE SHIT







        
        public async Task OnTick()
        {

        }

        public void OnPlayerSpawn(object obj)
        {
            TriggerEvent("chatMessage", "", new[] { 255, 255, 255 }, "/voice distance 12m");

        }

        public void OnPlayerLeftVehicle(object vehcile)
        {
            Screen.ShowNotification("Drücke 'E' 2 Sekunden um den Motor auszuschalten und dein Fahrzeug abzuschließen.");

        }






        //UI


        public void AddVehicleSpawner(UIMenu menu)
        {
            var submenu = _copMenuPool.AddSubMenu(menu, "Fahrzeuge", "Parke Fahrzeuge aus");

        }
        public void AddWeaponSpawner(UIMenu menu)
        {
            var submenu = _copMenuPool.AddSubMenu(menu, "Waffenmenü", "Rüste dich aus");


            var handfeuerwaffen = new UIMenuItem("Handfeuerwaffen", "Taser und Glock");
            submenu.AddItem(handfeuerwaffen);

            var m4a1 = new UIMenuItem("M4A1", "Maschienengewehr");
            submenu.AddItem(m4a1);

            var pumpShot = new UIMenuItem("Pump-Action-Shotgun", "Shotgun");
            submenu.AddItem(pumpShot);

            var weglegen = new UIMenuItem("~r~Waffen ablegen~r~", "Lege all deine Waffen ab");
            submenu.AddItem(weglegen);


            submenu.OnItemSelect += (sender, selectedItem, index) =>
            {
                if (selectedItem == handfeuerwaffen)
                {
                    LocalPlayer.Character.Weapons.Give(WeaponHash.StunGun, 1, false, true);
                    LocalPlayer.Character.Weapons.Give(WeaponHash.CombatPistol, 150, false, true);

                    Screen.ShowNotification("Waffen ausgerüstet.", true);
                }
                else if (selectedItem == m4a1)
                {
                    LocalPlayer.Character.Weapons.Give(WeaponHash.CarbineRifle, 200, true, true);
                    Screen.ShowNotification("Waffen ausgerüstet.", true);
                }
                else if (selectedItem == pumpShot)
                {
                    LocalPlayer.Character.Weapons.Give(WeaponHash.PumpShotgun, 200, true, true);
                    Screen.ShowNotification("Waffen ausgerüstet.", true);
                }
                else if (selectedItem == weglegen)
                {
                    LocalPlayer.Character.Weapons.RemoveAll();
                    Screen.ShowNotification("~r~Waffen abgelegt~r~");
                }
            };

        }

        public void AddVehicleConotrol(UIMenu menu)
        {
            var engineControl = new UIMenuItem("Motor ein/ausschalten", "Starte oder stoppe deinen Motor");
            menu.AddItem(engineControl);

            var lockVehicle = new UIMenuItem("Abschließen", "Schließe dein Fahrzeug ab");
            menu.AddItem(lockVehicle);

            var unlockVehicle = new UIMenuItem("Aufschließen", "Schließe dein Fahrzeug auf");
            menu.AddItem(unlockVehicle);

            var doorSubmenu = _carMenuPool.AddSubMenu(menu, "Türen", "Öffne oder schließe deine Türen");

            var repairCommand = new UIMenuItem("/REPAIR", "PLACEHOLDER");
            menu.AddItem(repairCommand);





            menu.OnItemSelect += (sender, selectedItem, index) =>
            {
                if (LocalPlayer.Character.CurrentVehicle.Driver == LocalPlayer.Character)
                {
                    if (selectedItem == engineControl)
                    {
                        if (LocalPlayer.Character.CurrentVehicle.IsEngineRunning == false)
                        {
                            API.SetVehicleEngineOn(LocalPlayer.Character.CurrentVehicle.Handle, true, true, true);
                            Screen.ShowNotification("Motor eingeschaltet");
                        }
                        else
                        {
                            API.SetVehicleEngineOn(LocalPlayer.Character.CurrentVehicle.Handle, false, true, true);
                            Screen.ShowNotification("Motor ausgeschaltet");

                        }
                    }
                    else if (selectedItem == lockVehicle)
                    {
                        API.SetVehicleDoorsLockedForAllPlayers(vehicle, true);
                        API.SetVehicleAllowNoPassengersLockon(vehicle, true);
                    }
                    else if(selectedItem == unlockVehicle)
                    {
                        API.SetVehicleDoorsLockedForAllPlayers(vehicle, false);
                        API.SetVehicleAllowNoPassengersLockon(vehicle, false);
                    }
                    else
                    {
                        Screen.ShowNotification("~r~Du bist nicht der Fahrer des Fahrzeugs!~r~");
                    }
                }
            };
        }


        //Create Menu

        public void CopMenu()
        {
            _copMenuPool = new MenuPool();
            var mainMenu = new UIMenu("Cop Menu", "~b~POLICE");
            _copMenuPool.Add(mainMenu);

            AddVehicleSpawner(mainMenu);
            AddWeaponSpawner(mainMenu);

            _copMenuPool.RefreshIndex();

            Tick += new Func<Task>(async delegate
            {
                _copMenuPool.ProcessMenus();
                if (Game.IsControlJustReleased(1, CitizenFX.Core.Control.ReplayScreenshot)) // Switch (U)
                {
                    mainMenu.Visible = !mainMenu.Visible;
                }
            });
        }

        public void CarMenu()
        {
            _carMenuPool = new MenuPool();
            var carMainMenu = new UIMenu("Autoschlüssel", "~b~PLATZHALTER");
            _carMenuPool.Add(carMainMenu);

            AddVehicleConotrol(carMainMenu);
            _carMenuPool.RefreshIndex();

            Tick += new Func<Task>(async delegate
            {
                _carMenuPool.ProcessMenus();
                if (Game.IsControlJustReleased(1, CitizenFX.Core.Control.InteractionMenu)) // Switch (M)
                {
                    carMainMenu.Visible = !carMainMenu.Visible;
                }
            });

        }

        public LOMCoreClient()
        {

            EventHandlers["playerSpawned"] += new Action<dynamic>(OnPlayerSpawn);
            EventHandlers["playerLeftVehicle"] += new Action<object>(OnPlayerLeftVehicle);
            EventHandlers["playerEnteredVehicle"] += new Action<object>(OnPlayerEnteredVehicle);

            Tick += OnTick;

            CarMenu();
            CopMenu();
            
        }






    }
}
