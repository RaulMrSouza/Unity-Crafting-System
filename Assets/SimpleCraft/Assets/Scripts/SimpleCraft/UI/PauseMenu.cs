using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleCraft.UI{
    /// <summary>
    /// Author: Raul Souza
    /// </summary>
	public class PauseMenu : MonoBehaviour {

		public void Quit(){
			Application.Quit();
		}

		public void Restart(){
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}
}