using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;


namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        public GameObject playernames;
        public Canvas canvas;
        private bool gamejoined;
        
        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion


        #region Public Methods


        public void DoStartGame()
        {
            PhotonNetwork.LoadLevel("Room for 2");
            gamejoined = true;

        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion
        
        #region Private Methods

        


        #endregion
        
        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            if (gamejoined == false)
            {
                PhotonNetwork.LoadLevel("Waiting Room " + PhotonNetwork.CurrentRoom.PlayerCount);
            }
            
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            if (gamejoined == false)
            {
                PhotonNetwork.LoadLevel("Waiting Room " + PhotonNetwork.CurrentRoom.PlayerCount);
            }
        }


        #endregion

        #region MonoBehavior

        private void Start()
        {
            int y = Screen.height/4*3-80;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var play = Instantiate(playernames, new Vector3(Screen.width/2,y,0),Quaternion.identity,canvas.transform);
                play.GetComponent<Text>().text = player.ToString();
                y -= 70;
            }
            
        }
        

        #endregion
    }
}