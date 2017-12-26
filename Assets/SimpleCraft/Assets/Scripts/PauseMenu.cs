using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleCraft{
	public class PauseMenu : MonoBehaviour {

		public void Quit(){
			Application.Quit();
		}

		public void Restart(){
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}
}