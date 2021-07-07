using System.Collections.Generic;
using UnityEngine;

namespace Class_Hierarchy
{
    public enum UnitType
    {
        Rifleman,
        Sargeant,
        Beetle,
        Sniper,
        Engineer,
        Queen,
        
    }

    public class Upgrades : MonoBehaviour
    {

        private Building Den;
        private Game game;
        private GameObject camera;

        private void Same()
        {
            Den.UpgradeMenu.SetActive(false);
            game.GetComponent<Game>().WaitForAction = false;
            var units = GameObject.FindGameObjectsWithTag("Unit");
            foreach (var unit in units)
            {
                unit.GetComponent<Units>().paused = false;
            }
            camera.GetComponent<CameraController>().paused = false;
        }

        public void Workforce()
        {
            if (game.wood>=30)
            {
                game.wood -= 30;
                Den.woodbyturn += 25;
                Den.WoodUpgrade = true;
                Same();
            }
        }
        
        public void Cattle()
        {
            if (game.wood>=30)
            {
                game.wood -= 30;
                Den.foodbyturn += 25;
                Den.FoodUpgrade = true;
                Same();
            }
            
        }
        public void Kitchens()
        {
            if (game.wood>=60)
            {
                game.wood -= 60;
                Den.royaljellybyturn += 5;
                Den.RoyalJellyUpgrade = true;
                Same();
            }
        }
        public void Reinforce()
        {
            if (game.wood>=60)
            {
                game.wood -= 60;
                Den.health += 50;
                Den.Maxhealth = Den.health;
                Den.HPUpgrades = true;
                Same();
            }
        }
        
        public void UnlockUnit()
        {
            UnitType type = Den.UnlockablesUnits.Peek();
            switch (type)
            {
                case UnitType.Sargeant:
                    if (game.wood >= 50)
                    {
                        game.wood -= 50;
                        Den.UnlockablesUnits.Dequeue();
                        Den.UnlockedUnits.Add(game.sargeant);
                        Same();
                    }
                    
                    break;
                case UnitType.Beetle :
                    if (game.wood >= 150)
                    {
                        game.wood -= 150;
                        Den.UnlockablesUnits.Dequeue();
                        Den.UnlockedUnits.Add(game.beetle);
                        Same();
                    }

                    break;
                case UnitType.Sniper:
                    if (game.wood >= 80)
                    {
                        game.wood -= 80;
                        Den.UnlockablesUnits.Dequeue();
                        Den.UnlockedUnits.Add(game.sniper);
                        Same();
                    }

                    break;
                case UnitType.Engineer:
                    if (game.wood >= 50)
                    {
                        game.wood -= 50;
                        Den.UnlockablesUnits.Dequeue();
                        Den.UnlockedUnits.Add(game.engineer);
                        Same();
                    }

                    break;
            }
            
        }

        void Start()
        {
            Den = transform.parent.gameObject.GetComponent<Building>();
            game = GameObject.Find("Game").GetComponent<Game>();
            camera = GameObject.Find("Camera");
        }
        
    }
}