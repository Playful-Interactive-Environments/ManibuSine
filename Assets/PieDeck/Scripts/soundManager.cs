using UnityEngine;
using System.Collections;

public class soundManager : MonoBehaviour {		
		
		public static AudioSource background, enemyDestroy, enemyCapture, msSpawn, msDamage, msTeleport, msIdle, plBlue, plRed,
	vrShoot, vrPlatform, baseSpin, baseHit, gameStart, gameWin, gameEnd, v_enemySpotted, v_tenPercent, v_thirtyPercent,
	v_fiftyPercent, v_eightyPercent, v_ninetyPercent, v_killEnemies, v_netSet, v_coreCharged, v_noEnergy, v_come, v_mothershipRange;

		// Use this for initialization
		void Start ()
		{
			
			AudioSource[] audios = GetComponents<AudioSource> ();
			background = audios [0];
			enemyDestroy = audios [1];
			enemyCapture = audios [2];
			msSpawn = audios [3];
			msDamage = audios [4];
			msTeleport = audios [5];
			msIdle = audios [6];
			plBlue = audios [7];
			plRed = audios [8];
			vrShoot = audios [9];
			vrPlatform = audios [10];
			baseSpin = audios [11];
			baseHit = audios [12];
			gameStart = audios [13];
			gameWin = audios [14];
			gameEnd = audios [15];
			v_enemySpotted = audios [16];
			v_tenPercent = audios [17];
			v_thirtyPercent = audios [18];
			v_fiftyPercent = audios [19];
			v_eightyPercent = audios [20];
			v_ninetyPercent = audios [21];
			v_killEnemies = audios [22];
			v_netSet = audios [23];
			v_coreCharged = audios [24];
			v_noEnergy = audios [25];
			v_come = audios [26];
			v_mothershipRange = audios [27];
				
			background.loop = true;
			background.volume = 0.075f;
			background.Play (); 
			
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
		
		public static void PlaySound (string name, float pan, float volume)
		{
		if (name == "enemyDestroy") {
				enemyDestroy.panStereo = pan;
				enemyDestroy.volume = volume;
				enemyDestroy.Play ();
				
			}
		if (name == "enemyCapture") {
				enemyCapture.panStereo = pan;
				enemyCapture.volume = volume;
				enemyCapture.Play ();
				
			}
		if (name == "msSpawn") {
			msSpawn.panStereo = pan;
			msSpawn.volume = volume;
			msSpawn.Play ();
				
			}
		if (name == "msDamage") {
			msDamage.panStereo = pan;
			msDamage.volume = volume;
			msDamage.Play ();
				
			}
		if (name == "msTeleport") {
			msTeleport.panStereo = pan;
			msTeleport.volume = volume;
			msTeleport.Play ();
				
			}
		if (name == "msIdle") {
			msIdle.panStereo = pan;
			msIdle.volume = volume;
			msIdle.loop = true;
			msIdle.Play ();
			}
		if (name == "msIdleStop") {
			msIdle.Stop ();
		}
		if (name == "plBlue") {
			plBlue.panStereo = pan;
			plBlue.volume = volume;
			plBlue.Play ();
		}

		if (name == "plRed") {
			plRed.panStereo = pan;
			plRed.volume = volume;
			plRed.Play ();
			
			}
		if (name == "vrShoot") {
			vrShoot.panStereo = pan;
			vrShoot.volume = volume;
			vrShoot.Play ();
				
			}
		if (name == "vrPlatform") {
			vrPlatform.panStereo = pan;
			vrPlatform.volume = volume;
			vrPlatform.Play ();
				
			}
		if (name == "baseSpin") {
			baseSpin.panStereo = pan;
			baseSpin.volume = volume;
			baseSpin.loop = true;
			baseSpin.Play ();

		}
		if (name == "baseHit") {
			baseHit.panStereo = pan;
			baseHit.volume = volume;
			baseHit.Play ();
				
			}
		if (name == "gameStart") {
			gameStart.panStereo = pan;
			gameStart.volume = volume;
			gameStart.Play ();
				
			}
		if (name == "gameWin") {
			gameWin.panStereo = pan;
			gameWin.volume = volume;
			gameWin.Play ();
				
			}
		if (name == "gameEnd") {
			gameEnd.panStereo = pan;
			gameEnd.volume = volume;
			gameEnd.Play ();
		}
		if (name == "v_enemySpotted") {
			v_enemySpotted.panStereo = pan;
			v_enemySpotted.volume = volume;
			v_enemySpotted.Play ();
		}
		if (name == "v_tenPercent") {
			v_tenPercent.panStereo = pan;
			v_tenPercent.volume = volume;
			v_tenPercent.Play ();
		}
		if (name == "v_thirtyPercent") {
			v_thirtyPercent.panStereo = pan;
			v_thirtyPercent.volume = volume;
			v_thirtyPercent.Play ();
		}
		if (name == "v_fiftyPercent") {
			v_fiftyPercent.panStereo = pan;
			v_fiftyPercent.volume = volume;
			v_fiftyPercent.Play ();
		}
		if (name == "v_eightyPercent") {
			v_eightyPercent.panStereo = pan;
			v_eightyPercent.volume = volume;
			v_eightyPercent.Play ();
		}
		if (name == "v_ninetyPercent") {
			v_ninetyPercent.panStereo = pan;
			v_ninetyPercent.volume = volume;
			v_ninetyPercent.Play ();
		}
		if (name == "v_killEnemies") {
			v_killEnemies.panStereo = pan;
			v_killEnemies.volume = volume;
			v_killEnemies.Play ();
		}
		if (name == "v_netSet") {
			v_netSet.panStereo = pan;
			v_netSet.volume = volume;
			v_netSet.Play ();
		}
		if (name == "v_coreCharged") {
			v_coreCharged.panStereo = pan;
			v_coreCharged.volume = volume;
			v_coreCharged.Play ();
		}
		if (name == "v_noEnergy") {
			v_noEnergy.panStereo = pan;
			v_noEnergy.volume = volume;
			v_noEnergy.Play ();
		}
		if (name == "v_come") {
			v_come.panStereo = pan;
			v_come.volume = volume;
			v_come.Play ();
		}
		if (name == "v_mothershipRange") {
			v_mothershipRange.panStereo = pan;
			v_mothershipRange.volume = volume;
			v_mothershipRange.Play ();
		}
	}
	}
