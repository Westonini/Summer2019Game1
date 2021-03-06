﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ActionPhase : MonoBehaviour
{
    private DecisionPhase dP;

    [HideInInspector]
    public string characterTurn;

    [Header("Text Objects")]
    public TextMeshProUGUI ally1HealthChangeText;
    public TextMeshProUGUI ally1MissText;
    public TextMeshProUGUI ally1StatusEffectText;
    public TextMeshProUGUI ally2HealthChangeText;
    public TextMeshProUGUI ally2MissText;
    public TextMeshProUGUI ally2StatusEffectText;
    public TextMeshProUGUI ally3HealthChangeText;
    public TextMeshProUGUI ally3MissText;
    public TextMeshProUGUI ally3StatusEffectText;
    public TextMeshProUGUI enemy1HealthChangeText;
    public TextMeshProUGUI enemy1MissText;
    public TextMeshProUGUI enemy1StatusEffectText;
    public TextMeshProUGUI enemy2HealthChangeText;
    public TextMeshProUGUI enemy2MissText;
    public TextMeshProUGUI enemy2StatusEffectText;
    public TextMeshProUGUI enemy3HealthChangeText;
    public TextMeshProUGUI enemy3MissText;
    public TextMeshProUGUI enemy3StatusEffectText;

    [HideInInspector]
    public int ally1STRBuff = 0, ally2STRBuff = 0, ally3STRBuff = 0, enemy1STRBuff = 0, enemy2STRBuff = 0, enemy3STRBuff = 0;

    [HideInInspector]
    public int ally1PermSTRBuff, ally2PermSTRBuff, ally3PermSTRBuff, enemy1PermSTRBuff, enemy2PermSTRBuff, enemy3PermSTRBuff;

    [HideInInspector]
    public int ally1PermDEFBuff, ally2PermDEFBuff, ally3PermDEFBuff, enemy1PermDEFBuff, enemy2PermDEFBuff, enemy3PermDEFBuff;

    private int ally1DEFBuff = 0;
    private int ally2DEFBuff = 0;
    private int ally3DEFBuff = 0;
    private int enemy1DEFBuff = 0;
    private int enemy2DEFBuff = 0;
    private int enemy3DEFBuff = 0;

    [HideInInspector]
    public int giveAlly1BuffNextTurn = 0, giveAlly2BuffNextTurn = 0, giveAlly3BuffNextTurn = 0, giveEnemy1BuffNextTurn = 0, giveEnemy2BuffNextTurn = 0, giveEnemy3BuffNextTurn = 0;

    [HideInInspector]
    public bool ally1IsCharging = false, ally2IsCharging = false, ally3IsCharging = false, enemy1IsCharging = false, enemy2IsCharging = false, enemy3IsCharging = false;

    [HideInInspector]
    public bool defensivePhase = true;

    private int accuracy;

    private bool ally1HasSmokeBomb = false;
    private bool ally2HasSmokeBomb = false;
    private bool ally3HasSmokeBomb = false;

    private int bleedChance;
    [HideInInspector]
    public int ally1IsBleeding = 0, ally2IsBleeding = 0, ally3IsBleeding = 0, enemy1IsBleeding = 0, enemy2IsBleeding = 0, enemy3IsBleeding = 0;
    private bool ally1HasBled = false;
    private bool ally2HasBled = false;
    private bool ally3HasBled = false;
    private bool enemy1HasBled = false;
    private bool enemy2HasBled = false;
    private bool enemy3HasBled = false;

    private bool someoneIsBleeding = false;

    void Awake()
    {
        dP = GameObject.FindWithTag("CombatControl").GetComponent<DecisionPhase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterTurn = dP.ally1Name;
    }

    // Update is called once per frame
    void Update()
    {
        if (dP.actionPhase == true) //if the Action Phase is active, call the character functions.
        {
            //If any of the characters have bleeding values of 1 or 2; someoneIsBleeding is set to true.
            if (ally1IsBleeding == 1 || ally1IsBleeding == 2 || ally2IsBleeding == 1 || ally2IsBleeding == 2 || ally3IsBleeding == 1 || ally3IsBleeding == 2 || enemy1IsBleeding == 1 || enemy1IsBleeding == 2 || enemy2IsBleeding == 1 || enemy2IsBleeding == 2 || enemy3IsBleeding == 1 || enemy3IsBleeding == 2)
            {
                someoneIsBleeding = true;
            }
            else
            {
                someoneIsBleeding = false;
            }

            if (defensivePhase == true) //If it's the DefensivePhase
            {
                HeroProtagonistDefensive();
                GlassCannonDefensive();
                SupportMainDefensive();
                SlimeDefensive();
                SlimeKingDefensive();
            }
            else if (defensivePhase != true) //If it's the OffensivePhase
            {
                CancelTurnIfDead();

                HeroProtagonistOffensive();
                GlassCannonOffensive();
                SupportMainOffensive();
                SlimeOffensive();
                SlimeKingOffensive();

                Bleed();
            }
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {

        }*/
    }


    //Called when the characterTurn == "HeroProtagonist" and the move chosen was a defensive-type move.
    void HeroProtagonistDefensive() 
    {
        if (characterTurn == "HeroProtagonist")
        {
            //If HeroProtagonist is ally1...
            if (dP.ally1Name == "HeroProtagonist")
            {
                if (dP.ally1TargetSelected == dP.ally1Name && dP.ally1MoveSelected == "War Cry")
                {
                    giveAlly1BuffNextTurn += 7;
                }

                HeroProtagonistDefensiveAllyBranching(dP.ally1MoveSelected, dP.ally1TargetSelected);

                if (dP.ally1MoveSelected == "Defend")
                {
                    {
                        StartCoroutine(DoAlly1Animation("Buffed"));
                        ally1DEFBuff += 5;
                        StartCoroutine(ShowPositiveStatusEffect(ally1StatusEffectText, "DEF +"));
                    }
                }
                if (dP.ally1MoveSelected == "War Cry" || dP.ally1MoveSelected == "Bandage-Up" || dP.ally1MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly1Animation("BuffOrHealGiven"));
                    dP.ally1MoveSelected = "";
                    dP.ally1TargetSelected = "";
                }
            }

            //If HeroProtagonist is ally2...
            if (dP.ally2Name == "HeroProtagonist")
            {
                if (dP.ally2TargetSelected == dP.ally2Name && dP.ally2MoveSelected == "War Cry")
                {
                    giveAlly2BuffNextTurn += 7;
                }

                HeroProtagonistDefensiveAllyBranching(dP.ally2MoveSelected, dP.ally2TargetSelected);

                if (dP.ally2MoveSelected == "Defend")
                {
                    {
                        StartCoroutine(DoAlly2Animation("Buffed"));
                        ally2DEFBuff += 5;
                        StartCoroutine(ShowPositiveStatusEffect(ally2StatusEffectText, "DEF +"));
                    }
                }
                if (dP.ally2MoveSelected == "War Cry" || dP.ally2MoveSelected == "Bandage-Up" || dP.ally2MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly2Animation("BuffOrHealGiven"));
                    dP.ally2MoveSelected = "";
                    dP.ally2TargetSelected = "";
                }
            }

            //If HeroProtagonist is ally3...
            if (dP.ally3Name == "HeroProtagonist")
            {
                if (dP.ally3TargetSelected == dP.ally3Name && dP.ally3MoveSelected == "War Cry")
                {
                    giveAlly3BuffNextTurn += 7;
                }

                HeroProtagonistDefensiveAllyBranching(dP.ally3MoveSelected, dP.ally3TargetSelected);

                if (dP.ally3MoveSelected == "Defend")
                {
                    {
                        StartCoroutine(DoAlly3Animation("Buffed"));
                        ally3DEFBuff += 5;
                        StartCoroutine(ShowPositiveStatusEffect(ally3StatusEffectText, "DEF +"));
                    }
                }
                if (dP.ally3MoveSelected == "War Cry" || dP.ally3MoveSelected == "Bandage-Up" || dP.ally3MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly3Animation("BuffOrHealGiven"));
                    dP.ally3MoveSelected = "";
                    dP.ally3TargetSelected = "";
                }
            }

            ChangeCharacterTurnDefensivePhase(); //If the characterTurn was "HeroProtagonist", call the function.
        }
    }

    //Called when the characterTurn == "HeroProtagonist" and the move chosen was an offensive-type move.
    void HeroProtagonistOffensive() 
    {
        if (characterTurn == "HeroProtagonist")
        {
            //If HeroProtagonist is ally1...
            if (dP.ally1Name == "HeroProtagonist" && dP.ally1MoveSelected != "")
            {
                StartCoroutine(DoAlly1Animation("Attack"));
                HeroProtagonistOffensiveAllyBranching(dP.ally1MoveSelected, dP.ally1TargetSelected, ally1STRBuff);
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            //If HeroProtagonist is ally2...
            else if (dP.ally2Name == "HeroProtagonist" && dP.ally2MoveSelected != "")
            {
                StartCoroutine(DoAlly2Animation("Attack"));
                HeroProtagonistOffensiveAllyBranching(dP.ally2MoveSelected, dP.ally2TargetSelected, ally2STRBuff);
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            //If HeroProtagonist is ally3...
            else if (dP.ally3Name == "HeroProtagonist" && dP.ally3MoveSelected != "")
            {
                StartCoroutine(DoAlly3Animation("Attack"));
                HeroProtagonistOffensiveAllyBranching(dP.ally3MoveSelected, dP.ally3TargetSelected, ally3STRBuff);
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }
            
            else
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }
    }

    void HeroProtagonistDefensiveAllyBranching(string MoveSelected, string TargetSelected) //Used for ally branching in the HeroProtagonistDefensive function.
    {
        if (MoveSelected == "War Cry") //If the move selected is War Cry...
        {
            if (TargetSelected == dP.ally1Name) //If the target selected is ally1...
            {
                StartCoroutine(DoAlly1Animation("Buffed"));
                ally1STRBuff += 7;
                StartCoroutine(ShowPositiveStatusEffect(ally1StatusEffectText, "STR +"));
            }
            else if (TargetSelected == dP.ally2Name) //If the target selected is ally2...
            {
                StartCoroutine(DoAlly2Animation("Buffed"));
                ally2STRBuff += 7;
                StartCoroutine(ShowPositiveStatusEffect(ally2StatusEffectText, "STR +"));
            }
            else if (TargetSelected == dP.ally3Name) //If the target selected is ally3...
            {
                StartCoroutine(DoAlly3Animation("Buffed"));
                ally3STRBuff += 7;
                StartCoroutine(ShowPositiveStatusEffect(ally3StatusEffectText, "STR +"));
            }
        }
        else if (MoveSelected == "Bandage-Up") //If the move selected is Bandage-Up...
        {
            if (TargetSelected == dP.ally1Name) //If the target selected is ally1...
            {
                StartCoroutine(DoAlly1Animation("Healed"));
                dP.ally1Health += HealHealth(6);
                StartCoroutine(ShowHealingDealt(ally1HealthChangeText, 6));
            }
            else if (TargetSelected == dP.ally2Name) //If the target selected is ally2...
            {
                StartCoroutine(DoAlly2Animation("Healed"));
                dP.ally2Health += HealHealth(6);
                StartCoroutine(ShowHealingDealt(ally2HealthChangeText, 6));
            }
            else if (TargetSelected == dP.ally3Name) //If the target selected is ally3...
            {
                StartCoroutine(DoAlly3Animation("Healed"));
                dP.ally3Health += HealHealth(6);
                StartCoroutine(ShowHealingDealt(ally3HealthChangeText, 6));
            }
        }
    }

    void HeroProtagonistOffensiveAllyBranching(string MoveSelected, string TargetSelected, int STRbuff) //Used for ally branching in the HeroProtagonistOffensive function.
    {
        if (MoveSelected == "Sword Slash") //If the move selected is Sword Slash...
        {
            accuracy = Random.Range(1, 101);

            if (TargetSelected == dP.enemy1Name && accuracy <= 85) //If the target selected is enemy1 and the accuracy is 80 or below...
            {
                StartCoroutine(DoEnemy1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("SwordHit");
                dP.enemy1Health -= DealDamage((10 + STRbuff) - enemy1DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((10 + STRbuff) - enemy1DEFBuff)));
            }
            else if (TargetSelected == dP.enemy1Name && accuracy > 85) //If the target selected is enemy1 but the attack misses...
            {
                StartCoroutine(ShowMiss(enemy1MissText));
            }
            else if (TargetSelected == dP.enemy2Name && accuracy <= 85) //If the target selected is enemy2 and the accuracy is 80 or below...
            {
                StartCoroutine(DoEnemy2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("SwordHit");
                dP.enemy2Health -= DealDamage((10 + STRbuff) - enemy2DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((10 + STRbuff) - enemy2DEFBuff)));
            }
            else if (TargetSelected == dP.enemy2Name && accuracy > 85) //If the target selected is enemy2 but the attack misses...
            {
                StartCoroutine(ShowMiss(enemy2MissText));
            }
            else if (TargetSelected == dP.enemy3Name && accuracy <= 85) //If the target selected is enemy3 and the accuracy is 80 or below...
            {
                StartCoroutine(DoEnemy3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("SwordHit");
                dP.enemy3Health -= DealDamage((10 + STRbuff) - enemy3DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((10 + STRbuff) - enemy3DEFBuff)));
            }
            else if (TargetSelected == dP.enemy3Name && accuracy > 85) //If the target selected is enemy3 but the attack misses...
            {
                StartCoroutine(ShowMiss(enemy3MissText));
            }
        }

        else if (MoveSelected == "Windmill") //If the move selected is Windmill...
        {
            //Enemy1
            accuracy = Random.Range(1, 101);
            if (dP.enemy1Dead != true)
            {
                if (accuracy <= 85)
                {
                    StartCoroutine(DoEnemy1Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("SwordHit");
                    dP.enemy1Health -= DealDamage((5 + STRbuff) - enemy1DEFBuff);
                    StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((5 + STRbuff) - enemy1DEFBuff)));
                }
                else if (accuracy > 85)
                {
                    StartCoroutine(ShowMiss(enemy1MissText));
                }
            }



            //Enemy2
            accuracy = Random.Range(1, 101);
            if (dP.enemy2Dead != true)
            {
                if (accuracy <= 85)
                {
                    StartCoroutine(DoEnemy2Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("SwordHit");
                    dP.enemy2Health -= DealDamage((5 + STRbuff) - enemy2DEFBuff);
                    StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((5 + STRbuff) - enemy2DEFBuff)));
                }
                else if (accuracy > 85)
                {
                    StartCoroutine(ShowMiss(enemy2MissText));
                }
            }



            //Enemy3
            accuracy = Random.Range(1, 101);
            if (dP.enemy3Dead != true)
            {
                if (accuracy <= 85)
                {
                    StartCoroutine(DoEnemy3Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("SwordHit");
                    dP.enemy3Health -= DealDamage((5 + STRbuff) - enemy3DEFBuff);
                    StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((5 + STRbuff) - enemy3DEFBuff)));
                }
                else if (accuracy > 85)
                {
                    StartCoroutine(ShowMiss(enemy3MissText));
                }
            }    
        }
    }

    //Called when the characterTurn == "GlassCannon" and the move chosen was a defensive-type move.
    void GlassCannonDefensive()
    {
        if (characterTurn == "GlassCannon")
        {
            //If GlassCannon is ally1...
            if (dP.ally1Name == "GlassCannon")
            {
                GlassCannonDefensiveAllyBranching(dP.ally1MoveSelected, dP.ally1TargetSelected);

                if (dP.ally1MoveSelected == "Defend") //If the move selected is Defend...
                {
                    StartCoroutine(DoAlly1Animation("Buffed"));
                    ally1DEFBuff += 5;
                    StartCoroutine(ShowPositiveStatusEffect(ally1StatusEffectText, "DEF +"));
                }
                if (dP.ally1MoveSelected == "Smoke Bomb" || dP.ally1MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly1Animation("BuffOrHealGiven"));
                    dP.ally1MoveSelected = "";
                    dP.ally1TargetSelected = "";
                }
            }


            //If GlassCannon is ally2...
            if (dP.ally2Name == "GlassCannon")
            {
                GlassCannonDefensiveAllyBranching(dP.ally2MoveSelected, dP.ally2TargetSelected);

                if (dP.ally2MoveSelected == "Defend") //If the move selected is Defend...
                {
                    StartCoroutine(DoAlly2Animation("Buffed"));
                    ally2DEFBuff += 5;
                    StartCoroutine(ShowPositiveStatusEffect(ally2StatusEffectText, "DEF +"));
                }
                if (dP.ally2MoveSelected == "Smoke Bomb" || dP.ally2MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly2Animation("BuffOrHealGiven"));
                    dP.ally2MoveSelected = "";
                    dP.ally2TargetSelected = "";
                }
            }

            //If GlassCannon is ally3...
            if (dP.ally3Name == "GlassCannon")
            {
                GlassCannonDefensiveAllyBranching(dP.ally3MoveSelected, dP.ally3TargetSelected);

                if (dP.ally3MoveSelected == "Defend") //If the move selected is Defend...
                {
                    StartCoroutine(DoAlly3Animation("Buffed"));
                    ally3DEFBuff += 5;
                    StartCoroutine(ShowPositiveStatusEffect(ally3StatusEffectText, "DEF +"));
                }
                if (dP.ally3MoveSelected == "Smoke Bomb" || dP.ally3MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly3Animation("BuffOrHealGiven"));
                    dP.ally3MoveSelected = "";
                    dP.ally3TargetSelected = "";
                }
            }
            ChangeCharacterTurnDefensivePhase(); //If the characterTurn was "GlassCannon", call the function.
        }
    }

    //Called when the characterTurn == "GlassCannon" and the move chosen was an offensive-type move.
    void GlassCannonOffensive()
    {
        if (characterTurn == "GlassCannon")
        {
            //If GlassCannon is ally1...
            if (dP.ally1Name == "GlassCannon" && dP.ally1MoveSelected != "")
            {
                if ((dP.ally1MoveSelected == "Focus Shot" && ally1IsCharging != false) || dP.ally1MoveSelected == "Shard Shot" || dP.ally1MoveSelected == "Shatter")
                {
                    StartCoroutine(DoAlly1Animation("Attack"));
                }

                GlassCannonOffensiveAllyBranching(dP.ally1MoveSelected, dP.ally1TargetSelected, ally1STRBuff, "Ally1");

                if (dP.ally1MoveSelected != "Focus Shot")
                {
                    dP.ally1MoveSelected = "";
                    dP.ally1TargetSelected = "";
                }
                ChangeCharacterTurnOffensivePhase();
            }

            //If GlassCannon is ally2...
            else if (dP.ally2Name == "GlassCannon" && dP.ally2MoveSelected != "")
            {
                if ((dP.ally2MoveSelected == "Focus Shot" && ally2IsCharging != false) || dP.ally2MoveSelected == "Shard Shot" || dP.ally2MoveSelected == "Shatter")
                {
                    StartCoroutine(DoAlly2Animation("Attack"));
                }

                GlassCannonOffensiveAllyBranching(dP.ally2MoveSelected, dP.ally2TargetSelected, ally2STRBuff, "Ally2");

                if (dP.ally2MoveSelected != "Focus Shot")
                {
                    dP.ally2MoveSelected = "";
                    dP.ally2TargetSelected = "";
                }
                ChangeCharacterTurnOffensivePhase();
            }

            //If GlassCannon is ally3...
            else if (dP.ally3Name == "GlassCannon" && dP.ally3MoveSelected != "")
            {
                if ((dP.ally3MoveSelected == "Focus Shot" && ally3IsCharging != false) || dP.ally3MoveSelected == "Shard Shot" || dP.ally3MoveSelected == "Shatter")
                {
                    StartCoroutine(DoAlly3Animation("Attack"));
                }

                GlassCannonOffensiveAllyBranching(dP.ally3MoveSelected, dP.ally3TargetSelected, ally3STRBuff, "Ally3");

                if (dP.ally3MoveSelected != "Focus Shot")
                {
                    dP.ally3MoveSelected = "";
                    dP.ally3TargetSelected = "";
                }
                ChangeCharacterTurnOffensivePhase();
            }

            else
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }
    }

    void GlassCannonDefensiveAllyBranching(string MoveSelected, string TargetSelected) //Used for ally branching in the GlassCannonDefensive function.
    {
        if (MoveSelected == "Smoke Bomb") //If the move selected is Smoke bomb...
        {
            if (TargetSelected == dP.ally1Name) //If ally1 is targeted...
            {
                StartCoroutine(DoAlly1Animation("Buffed"));
                ally1HasSmokeBomb = true;
                StartCoroutine(ShowPositiveStatusEffect(ally1StatusEffectText, "EVADE +"));
            }
            else if (TargetSelected == dP.ally2Name) //If ally2 is targeted...
            {
                StartCoroutine(DoAlly2Animation("Buffed"));
                ally2HasSmokeBomb = true;
                StartCoroutine(ShowPositiveStatusEffect(ally2StatusEffectText, "EVADE +"));
            }
            else if (TargetSelected == dP.ally3Name) //If ally3 is targeted...
            {
                StartCoroutine(DoAlly3Animation("Buffed"));
                ally3HasSmokeBomb = true;
                StartCoroutine(ShowPositiveStatusEffect(ally3StatusEffectText, "EVADE +"));
            }
        }
    }

    void GlassCannonOffensiveAllyBranching(string MoveSelected, string TargetSelected, int STRbuff, string AllyPlacement) //Used for ally branching in the GlassCannonOffensive function.
    {
        if (MoveSelected == "Shard Shot") // If the move selected is Shard Shot...
        {
            accuracy = Random.Range(1, 101);
            bleedChance = Random.Range(1, 101);

            if (TargetSelected == dP.enemy1Name && accuracy <= 75 && bleedChance > 30) //If the target selected is enemy1 and the accuracy is 70 or below...
            {
                StartCoroutine(DoEnemy1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy1Health -= DealDamage((15 + STRbuff) - enemy1DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((15 + STRbuff) - enemy1DEFBuff)));
            }
            else if (TargetSelected == dP.enemy1Name && accuracy <= 75 && bleedChance <= 30) //If the target selected is enemy1 and the accuracy is 70 or below and the bleedChance is 30 or below...
            {
                StartCoroutine(DoEnemy1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy1Health -= DealDamage((15 + STRbuff) - enemy1DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((15 + STRbuff) - enemy1DEFBuff)));
                enemy1IsBleeding = 1;
                StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
            }
            else if (TargetSelected == dP.enemy1Name && accuracy > 75) //If the target selected is enemy1 but it misses...
            {
                StartCoroutine(ShowMiss(enemy1MissText));
            }
            else if (TargetSelected == dP.enemy2Name && accuracy <= 75 && bleedChance > 30) //If the target selected is enemy2 and the accuracy is 70 or below...
            {
                StartCoroutine(DoEnemy2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy2Health -= DealDamage((15 + STRbuff) - enemy2DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((15 + STRbuff) - enemy2DEFBuff)));
            }
            else if (TargetSelected == dP.enemy2Name && accuracy <= 75 && bleedChance <= 30) //If the target selected is enemy2 and the accuracy is 70 or below and the bleedChance is 30 or below...
            {
                StartCoroutine(DoEnemy2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy2Health -= DealDamage((15 + STRbuff) - enemy2DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((15 + STRbuff) - enemy2DEFBuff)));
                enemy2IsBleeding = 1;
                StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
            }
            else if (TargetSelected == dP.enemy2Name && accuracy > 75) //If the target selected is enemy2 but it misses...
            {
                StartCoroutine(ShowMiss(enemy2MissText));
            }
            else if (TargetSelected == dP.enemy3Name && accuracy <= 75 && bleedChance > 30) //If the target selected is enemy3 and the accuracy is 70 or below...
            {
                StartCoroutine(DoEnemy3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy3Health -= DealDamage((15 + STRbuff) - enemy3DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((15 + STRbuff) - enemy3DEFBuff)));
            }
            else if (TargetSelected == dP.enemy3Name && accuracy <= 75 && bleedChance <= 30) //If the target selected is enemy3 and the accuracy is 70 or below and the bleedChance is 30 or below...
            {
                StartCoroutine(DoEnemy3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy3Health -= DealDamage((15 + STRbuff) - enemy3DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((15 + STRbuff) - enemy3DEFBuff)));
                enemy3IsBleeding = 1;
                StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
            }
            else if (TargetSelected == dP.enemy3Name && accuracy > 75) //If the target selected is enemy3 but it misses...
            {
                StartCoroutine(ShowMiss(enemy3MissText));
            }
        }

        else if (MoveSelected == "Shatter") //If the move selected is Shatter...
        {
            //Enemy1
            accuracy = Random.Range(1, 101);
            bleedChance = Random.Range(1, 101);

            if (dP.enemy1Dead != true)
            {
                if (accuracy <= 75) //If the accuracy is 70 or lower it hits
                {
                    StartCoroutine(DoEnemy1Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("Cannon");
                    dP.enemy1Health -= DealDamage((7 + STRbuff) - enemy1DEFBuff);
                    StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((7 + STRbuff) - enemy1DEFBuff)));
                    if (bleedChance <= 15)
                    {
                        enemy1IsBleeding = 1;
                        StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                    }
                }
                else if (accuracy > 75) //If the accuracy is 70 or lower it misses
                {
                    StartCoroutine(ShowMiss(enemy1MissText));
                }
            }


            //Enemy2
            accuracy = Random.Range(1, 101);
            bleedChance = Random.Range(1, 101);

            if (dP.enemy2Dead != true)
            {
                if (accuracy <= 75) //If the accuracy is 70 or lower it hits
                {
                    StartCoroutine(DoEnemy2Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("Cannon");
                    dP.enemy2Health -= DealDamage((7 + STRbuff) - enemy2DEFBuff);
                    StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((7 + STRbuff) - enemy2DEFBuff)));
                    if (bleedChance <= 15)
                    {
                        enemy2IsBleeding = 1;
                        StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                    }
                }
                else if (accuracy > 75) //If the accuracy is 70 or lower it misses
                {
                    StartCoroutine(ShowMiss(enemy2MissText));
                }
            }


            //Enemy3
            accuracy = Random.Range(1, 101);
            bleedChance = Random.Range(1, 101);

            if (dP.enemy3Dead != true)
            {
                if (accuracy <= 75) //If the accuracy is 70 or lower it hits
                {
                    StartCoroutine(DoEnemy3Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("Cannon");
                    dP.enemy3Health -= DealDamage((7 + STRbuff) - enemy3DEFBuff);
                    StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((7 + STRbuff) - enemy3DEFBuff)));
                    if (bleedChance <= 15)
                    {
                        enemy3IsBleeding = 1;
                        StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                    }
                }
                else if (accuracy > 75) //If the accuracy is 70 or lower it misses
                {
                    StartCoroutine(ShowMiss(enemy3MissText));
                }
            }
        }

        else if (MoveSelected == "Focus Shot") //If the move selected is Focus Shot...
        {          
            accuracy = Random.Range(1, 101);
            bleedChance = Random.Range(1, 101);

            //If GlassCannon is Ally1 and hasn't charged yet...
            if (AllyPlacement == "Ally1" && ally1IsCharging == false)
            {
                StartCoroutine(DoAlly1Animation("BuffOrHealGiven"));
                StartCoroutine(DoAlly1Animation("Buffed"));
                ally1IsCharging = true;
                StartCoroutine(ShowPositiveStatusEffect(ally1StatusEffectText, "Charging"));
            }
            //Else if GlassCannon is Ally1 but has already charged...
            else if (AllyPlacement == "Ally1" && ally1IsCharging == true && TargetSelected == dP.enemy1Name && accuracy <= 75) //If the target selected is enemy1 and it hits...
            {
                StartCoroutine(DoEnemy1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy1Health -= DealDamage((35 + STRbuff) - enemy1DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((35 + STRbuff) - enemy1DEFBuff)));
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ally1IsCharging = false;              
                if (bleedChance <= 40)
                {
                    enemy1IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally1" && ally1IsCharging == true && TargetSelected == dP.enemy1Name && accuracy > 75) //If the target selected is enemy1 and it misses...
            {
                StartCoroutine(ShowMiss(enemy1MissText));
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ally1IsCharging = false;
            }
            else if (AllyPlacement == "Ally1" && ally1IsCharging == true && TargetSelected == dP.enemy2Name && accuracy <= 75) //If the target selected is enemy2 and it hits...
            {
                StartCoroutine(DoEnemy2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy2Health -= DealDamage((35 + STRbuff) - enemy2DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((35 + STRbuff) - enemy2DEFBuff)));
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ally1IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy2IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally1" && ally1IsCharging == true && TargetSelected == dP.enemy2Name && accuracy > 75) //If the target selected is enemy2 and it misses...
            {
                StartCoroutine(ShowMiss(enemy2MissText));
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ally1IsCharging = false;
            }
            else if (AllyPlacement == "Ally1" && ally1IsCharging == true && TargetSelected == dP.enemy3Name && accuracy <= 75) //If the target selected is enemy3 and it hits...
            {
                StartCoroutine(DoEnemy3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy3Health -= DealDamage((35 + STRbuff) - enemy3DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((35 + STRbuff) - enemy3DEFBuff)));
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ally1IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy3IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally1" && ally1IsCharging == true && TargetSelected == dP.enemy3Name && accuracy > 75) //If the target selected is enemy3 and it misses...
            {
                StartCoroutine(ShowMiss(enemy3MissText));
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ally1IsCharging = false;
            }


            //If GlassCannon is Ally2 and hasn't charged yet...
            if (AllyPlacement == "Ally2" && ally2IsCharging == false)
            {
                StartCoroutine(DoAlly2Animation("BuffOrHealGiven"));
                StartCoroutine(DoAlly2Animation("Buffed"));
                ally2IsCharging = true;
                StartCoroutine(ShowPositiveStatusEffect(ally2StatusEffectText, "Charging"));
            }
            //Else if GlassCannon is Ally2 but has already charged...
            else if (AllyPlacement == "Ally2" && ally2IsCharging == true && TargetSelected == dP.enemy1Name && accuracy <= 75) //If the target selected is enemy1 and it hits...
            {
                StartCoroutine(DoEnemy1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy1Health -= DealDamage((35 + STRbuff) - enemy1DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((35 + STRbuff) - enemy1DEFBuff)));
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ally2IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy1IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally2" && ally2IsCharging == true && TargetSelected == dP.enemy1Name && accuracy > 75) //If the target selected is enemy1 and it misses...
            {
                StartCoroutine(ShowMiss(enemy1MissText));
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ally2IsCharging = false;
            }
            else if (AllyPlacement == "Ally2" && ally2IsCharging == true && TargetSelected == dP.enemy2Name && accuracy <= 75) //If the target selected is enemy2 and it hits...
            {
                StartCoroutine(DoEnemy2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy2Health -= DealDamage((35 + STRbuff) - enemy2DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((35 + STRbuff) - enemy2DEFBuff)));
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ally2IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy2IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally2" && ally2IsCharging == true && TargetSelected == dP.enemy2Name && accuracy > 75) //If the target selected is enemy2 and it misses...
            {
                StartCoroutine(ShowMiss(enemy2MissText));
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ally2IsCharging = false;
            }
            else if (AllyPlacement == "Ally2" && ally2IsCharging == true && TargetSelected == dP.enemy3Name && accuracy <= 75) //If the target selected is enemy3 and it hits...
            {
                StartCoroutine(DoEnemy3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy3Health -= DealDamage((35 + STRbuff) - enemy3DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((35 + STRbuff) - enemy3DEFBuff)));
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ally2IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy3IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally2" && ally2IsCharging == true && TargetSelected == dP.enemy3Name && accuracy > 75) //If the target selected is enemy3 and it misses...
            {
                StartCoroutine(ShowMiss(enemy3MissText));
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ally2IsCharging = false;
            }


            //If GlassCannon is Ally3 and hasn't charged yet...
            if (AllyPlacement == "Ally3" && ally3IsCharging == false)
            {
                StartCoroutine(DoAlly3Animation("BuffOrHealGiven"));
                StartCoroutine(DoAlly3Animation("Buffed"));
                ally3IsCharging = true;
                StartCoroutine(ShowPositiveStatusEffect(ally3StatusEffectText, "Charging"));
            }
            //Else if GlassCannon is Ally3 but has already charged...
            else if (AllyPlacement == "Ally3" && ally3IsCharging == true && TargetSelected == dP.enemy1Name && accuracy <= 75) //If the target selected is enemy1 and it hits...
            {
                StartCoroutine(DoEnemy1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy1Health -= DealDamage((35 + STRbuff) - enemy1DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((35 + STRbuff) - enemy1DEFBuff)));
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ally3IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy1IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally3" && ally3IsCharging == true && TargetSelected == dP.enemy1Name && accuracy > 75) //If the target selected is enemy1 and it misses...
            {
                StartCoroutine(ShowMiss(enemy1MissText));
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ally3IsCharging = false;
            }
            else if (AllyPlacement == "Ally3" && ally3IsCharging == true && TargetSelected == dP.enemy2Name && accuracy <= 75) //If the target selected is enemy2 and it hits...
            {
                StartCoroutine(DoEnemy2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy2Health -= DealDamage((35 + STRbuff) - enemy2DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((35 + STRbuff) - enemy2DEFBuff)));
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ally3IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy2IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally3" && ally3IsCharging == true && TargetSelected == dP.enemy2Name && accuracy > 75) //If the target selected is enemy2 and it misses...
            {
                StartCoroutine(ShowMiss(enemy2MissText));
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ally3IsCharging = false;
            }
            else if (AllyPlacement == "Ally3" && ally3IsCharging == true && TargetSelected == dP.enemy3Name && accuracy <= 75) //If the target selected is enemy3 and it hits...
            {
                StartCoroutine(DoEnemy3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Cannon");
                dP.enemy3Health -= DealDamage((35 + STRbuff) - enemy3DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((35 + STRbuff) - enemy3DEFBuff)));
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ally3IsCharging = false;
                if (bleedChance <= 40)
                {
                    enemy3IsBleeding = 1;
                    StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                }
            }
            else if (AllyPlacement == "Ally3" && ally3IsCharging == true && TargetSelected == dP.enemy3Name && accuracy > 75) //If the target selected is enemy3 and it misses...
            {
                StartCoroutine(ShowMiss(enemy3MissText));
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ally3IsCharging = false;
            }
        }
    }

    //Called when the characterTurn == "SupportMain" and the move chosen was a defensive-type move.
    void SupportMainDefensive()
    {
        if (characterTurn == "SupportMain")
        {
            //If SupportMain is ally1...
            if (dP.ally1Name == "SupportMain")
            {
                SupportMainDefensiveAllyBranching(dP.ally1MoveSelected, dP.ally1TargetSelected);

                if (dP.ally1MoveSelected == "Defend")
                {
                    {
                        StartCoroutine(DoAlly1Animation("BuffOrHealGiven"));
                        StartCoroutine(DoAlly1Animation("Buffed"));
                        ally1DEFBuff += 5;
                        StartCoroutine(ShowPositiveStatusEffect(ally1StatusEffectText, "DEF +"));
                    }
                }
                if (dP.ally1MoveSelected == "Mend" || dP.ally1MoveSelected == "Buckle Down" || dP.ally1MoveSelected == "Mend-All" || dP.ally1MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly1Animation("BuffOrHealGiven"));
                    dP.ally1MoveSelected = "";
                    dP.ally1TargetSelected = "";
                }
            }

            //If SupportMain is ally2...
            else if (dP.ally2Name == "SupportMain")
            {
                SupportMainDefensiveAllyBranching(dP.ally2MoveSelected, dP.ally2TargetSelected);

                if (dP.ally2MoveSelected == "Defend")
                {
                    {
                        StartCoroutine(DoAlly2Animation("BuffOrHealGiven"));
                        StartCoroutine(DoAlly2Animation("Buffed"));
                        ally2DEFBuff += 5;
                        StartCoroutine(ShowPositiveStatusEffect(ally2StatusEffectText, "DEF +"));
                    }
                }
                if (dP.ally2MoveSelected == "Mend" || dP.ally2MoveSelected == "Buckle Down" || dP.ally2MoveSelected == "Mend-All" || dP.ally2MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly2Animation("BuffOrHealGiven"));
                    dP.ally2MoveSelected = "";
                    dP.ally2TargetSelected = "";
                }
            }

            //If SupportMain is ally3...
            if (dP.ally3Name == "SupportMain")
            {
                SupportMainDefensiveAllyBranching(dP.ally3MoveSelected, dP.ally3TargetSelected);

                if (dP.ally3MoveSelected == "Defend")
                {
                    {
                        StartCoroutine(DoAlly3Animation("BuffOrHealGiven"));
                        StartCoroutine(DoAlly3Animation("Buffed"));
                        ally3DEFBuff += 5;
                        StartCoroutine(ShowPositiveStatusEffect(ally3StatusEffectText, "DEF +"));
                    }
                }
                if (dP.ally3MoveSelected == "Mend" || dP.ally3MoveSelected == "Buckle Down" || dP.ally3MoveSelected == "Mend-All" || dP.ally3MoveSelected == "Defend")
                {
                    StartCoroutine(DoAlly3Animation("BuffOrHealGiven"));
                    dP.ally3MoveSelected = "";
                    dP.ally3TargetSelected = "";
                }
            }

            ChangeCharacterTurnDefensivePhase(); //If the characterTurn was "SupportMain", call the function.
        }
    }

    //Called when the characterTurn == "SupportMain" and the move chosen was an offensive-type move.
    void SupportMainOffensive()
    {
        if (characterTurn == "SupportMain")
        {
            //If SupportMain is ally1...
            if (dP.ally1Name == "SupportMain" && dP.ally1MoveSelected != "")
            {
                StartCoroutine(DoAlly1Animation("Attack"));
                SupportMainOffensiveAllyBranching(dP.ally1MoveSelected, dP.ally1TargetSelected, ally1STRBuff, "Ally1");
                dP.ally1MoveSelected = "";
                dP.ally1TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            //If SupportMain is ally2...
            else if (dP.ally2Name == "SupportMain" && dP.ally2MoveSelected != "")
            {
                StartCoroutine(DoAlly2Animation("Attack"));
                SupportMainOffensiveAllyBranching(dP.ally2MoveSelected, dP.ally2TargetSelected, ally2STRBuff, "Ally2");
                dP.ally2MoveSelected = "";
                dP.ally2TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            //If SupportMain is ally3...
            else if (dP.ally3Name == "SupportMain" && dP.ally3MoveSelected != "")
            {
                StartCoroutine(DoAlly3Animation("Attack"));
                SupportMainOffensiveAllyBranching(dP.ally3MoveSelected, dP.ally3TargetSelected, ally3STRBuff, "Ally3");
                dP.ally3MoveSelected = "";
                dP.ally3TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            else
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }
    }

    void SupportMainDefensiveAllyBranching(string MoveSelected, string TargetSelected) //Used for ally branching in the SupportMainDefensive function.
    {
        if (MoveSelected == "Mend") //If the move selected is Mend...
        {
            if (TargetSelected == dP.ally1Name) //If the target selected is ally1...
            {
                StartCoroutine(DoAlly1Animation("Healed"));
                dP.ally1Health += HealHealth(12);
                StartCoroutine(ShowHealingDealt(ally1HealthChangeText, 12));
            }
            else if (TargetSelected == dP.ally2Name) //If the target selected is ally2...
            {
                StartCoroutine(DoAlly2Animation("Healed"));
                dP.ally2Health += HealHealth(12);
                StartCoroutine(ShowHealingDealt(ally2HealthChangeText, 12));
            }
            else if (TargetSelected == dP.ally3Name) //If the target selected is ally3...
            {
                StartCoroutine(DoAlly3Animation("Healed"));
                dP.ally3Health += HealHealth(12);
                StartCoroutine(ShowHealingDealt(ally3HealthChangeText, 12));
            }
        }

        else if (MoveSelected == "Buckle Down") //If the move selected is Buckle Down...
        {
            StartCoroutine(DoAlly1Animation("Buffed"));
            ally1DEFBuff += 5;
            StartCoroutine(ShowPositiveStatusEffect(ally1StatusEffectText, "DEF +"));

            StartCoroutine(DoAlly2Animation("Buffed"));
            ally2DEFBuff += 5;
            StartCoroutine(ShowPositiveStatusEffect(ally2StatusEffectText, "DEF +"));

            StartCoroutine(DoAlly3Animation("Buffed"));
            ally3DEFBuff += 5;
            StartCoroutine(ShowPositiveStatusEffect(ally3StatusEffectText, "DEF +"));
        }

        else if (MoveSelected == "Mend-All") //If the move selected is Mend-All...
        {
            StartCoroutine(DoAlly1Animation("Healed"));
            dP.ally1Health += HealHealth(6);
            StartCoroutine(ShowHealingDealt(ally1HealthChangeText, 6));

            StartCoroutine(DoAlly2Animation("Healed"));
            dP.ally2Health += HealHealth(6);
            StartCoroutine(ShowHealingDealt(ally2HealthChangeText, 6));

            StartCoroutine(DoAlly3Animation("Healed"));
            dP.ally3Health += HealHealth(6);
            StartCoroutine(ShowHealingDealt(ally3HealthChangeText, 6));
        }
    }

    void SupportMainOffensiveAllyBranching(string MoveSelected, string TargetSelected, int STRbuff, string AllyPlacement) //Used for ally branching in the SupportMainOffensive function.
    {
        if (MoveSelected == "Leech") //If the move selected is Leech...
        {
            accuracy = Random.Range(1, 101);

            if (TargetSelected == dP.enemy1Name && accuracy <= 90) //If the target selected is enemy1 and the accuracy is 80 or below...
            {
                StartCoroutine(DoEnemy1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Spell");
                dP.enemy1Health -= DealDamage((8 + STRbuff) - enemy1DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, ((8 + STRbuff) - enemy1DEFBuff)));

                if (AllyPlacement == "Ally1")
                {
                    StartCoroutine(DoAlly1Animation("Healed"));
                    dP.ally1Health += HealHealth(((8 + STRbuff) - enemy1DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally1HealthChangeText, ((8 + STRbuff) - enemy1DEFBuff) / 2));
                }
                else if (AllyPlacement == "Ally2")
                {
                    StartCoroutine(DoAlly2Animation("Healed"));
                    dP.ally2Health += HealHealth(((8 + STRbuff) - enemy1DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally2HealthChangeText, ((8 + STRbuff) - enemy1DEFBuff) / 2));
                }
                if (AllyPlacement == "Ally3")
                {
                    StartCoroutine(DoAlly3Animation("Healed"));
                    dP.ally3Health += HealHealth(((8 + STRbuff) - enemy1DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally3HealthChangeText, ((8 + STRbuff) - enemy1DEFBuff) / 2));
                }
            }
            else if (TargetSelected == dP.enemy1Name && accuracy > 90) //If the target selected is enemy1 but the attack misses...
            {
                StartCoroutine(ShowMiss(enemy1MissText));
            }

            else if (TargetSelected == dP.enemy2Name && accuracy <= 90) //If the target selected is enemy2 and the accuracy is 80 or below...
            {
                StartCoroutine(DoEnemy2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Spell");
                dP.enemy2Health -= DealDamage((8 + STRbuff) - enemy2DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, ((8 + STRbuff) - enemy2DEFBuff)));

                if (AllyPlacement == "Ally1")
                {
                    StartCoroutine(DoAlly1Animation("Healed"));
                    dP.ally1Health += HealHealth(((8 + STRbuff) - enemy2DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally1HealthChangeText, ((8 + STRbuff) - enemy2DEFBuff) / 2));
                }
                else if (AllyPlacement == "Ally2")
                {
                    StartCoroutine(DoAlly2Animation("Healed"));
                    dP.ally2Health += HealHealth(((8 + STRbuff) - enemy2DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally2HealthChangeText, ((8 + STRbuff) - enemy2DEFBuff) / 2));
                }
                if (AllyPlacement == "Ally3")
                {
                    StartCoroutine(DoAlly3Animation("Healed"));
                    dP.ally3Health += HealHealth(((8 + STRbuff) - enemy2DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally3HealthChangeText, ((8 + STRbuff) - enemy2DEFBuff) / 2));
                }
            }
            else if (TargetSelected == dP.enemy2Name && accuracy > 90) //If the target selected is enemy2 but the attack misses...
            {
                StartCoroutine(ShowMiss(enemy2MissText));
            }


            else if (TargetSelected == dP.enemy3Name && accuracy <= 90) //If the target selected is enemy3 and the accuracy is 80 or below...
            {
                StartCoroutine(DoEnemy3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Spell");
                dP.enemy3Health -= DealDamage((8 + STRbuff) - enemy3DEFBuff);
                StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, ((8 + STRbuff) - enemy3DEFBuff)));

                if (AllyPlacement == "Ally1")
                {
                    StartCoroutine(DoAlly1Animation("Healed"));
                    dP.ally1Health += HealHealth(((8 + STRbuff) - enemy3DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally1HealthChangeText, ((8 + STRbuff) - enemy3DEFBuff) / 2));
                }
                else if (AllyPlacement == "Ally2")
                {
                    StartCoroutine(DoAlly2Animation("Healed"));
                    dP.ally2Health += HealHealth(((8 + STRbuff) - enemy3DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally2HealthChangeText, ((8 + STRbuff) - enemy3DEFBuff) / 2));
                }
                if (AllyPlacement == "Ally3")
                {
                    StartCoroutine(DoAlly3Animation("Healed"));
                    dP.ally3Health += HealHealth(((8 + STRbuff) - enemy3DEFBuff) / 2);
                    StartCoroutine(ShowHealingDealt(ally3HealthChangeText, ((8 + STRbuff) - enemy3DEFBuff) / 2));
                }
            }
            else if (TargetSelected == dP.enemy3Name && accuracy > 90) //If the target selected is enemy3 but the attack misses...
            {
                StartCoroutine(ShowMiss(enemy3MissText));
            }
        }
    }

    //Called when the characterTurn == "Slime1/Slime2/Slime3" and the move chosen was an defensive-type move.
    void SlimeDefensive()
    {
        if (characterTurn == "Slime1") //If it's Slime1's turn...
        {
            //If Slime1 is enemy1...
            if (dP.enemy1Name == "Slime1")
            {
                if (dP.enemy1MoveSelected == "Power-Up" && enemy1PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy1Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy1Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy1StatusEffectText, "STR +"));
                    enemy1STRBuff += 2;
                    enemy1PermSTRBuff += 2;
                    dP.enemy1MoveSelected = "";
                    dP.enemy1TargetSelected = "";
                }
                else if (dP.enemy1MoveSelected == "Power-Up" && enemy1PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy1TargetSelected = "";
                    dP.enemy1MoveSelected = "Attack";
                    while ((dP.enemy1TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy1TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy1TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy1TargetSelected == "")
                    {
                        dP.enemy1TargetSelected = dP.EnemyTargetSelect(dP.enemy1MoveSelected, dP.enemy1Name);
                    }
                }
            }
            //If Slime1 is enemy2...
            else if (dP.enemy2Name == "Slime1")
            {
                if (dP.enemy2MoveSelected == "Power-Up" && enemy2PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy2Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy2Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy2StatusEffectText, "STR +"));
                    enemy2STRBuff += 2;
                    enemy2PermSTRBuff += 2;
                    dP.enemy2MoveSelected = "";
                    dP.enemy2TargetSelected = "";
                }
                else if (dP.enemy2MoveSelected == "Power-Up" && enemy2PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy2TargetSelected = "";
                    dP.enemy2MoveSelected = "Attack";
                    while ((dP.enemy2TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy2TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy2TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy2TargetSelected == "")
                    {
                        dP.enemy2TargetSelected = dP.EnemyTargetSelect(dP.enemy2MoveSelected, dP.enemy2Name);
                    }
                }
            }
            //If Slime1 is enemy3...
            if (dP.enemy3Name == "Slime1") 
            {
                if (dP.enemy3MoveSelected == "Power-Up" && enemy3PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy3Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy3Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy3StatusEffectText, "STR +"));
                    enemy3STRBuff += 2;
                    enemy3PermSTRBuff += 2;
                    dP.enemy3MoveSelected = "";
                    dP.enemy3TargetSelected = "";
                }
                else if (dP.enemy3MoveSelected == "Power-Up" && enemy3PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy3TargetSelected = "";
                    dP.enemy3MoveSelected = "Attack";
                    while ((dP.enemy3TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy3TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy3TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy3TargetSelected == "")
                    {
                        dP.enemy3TargetSelected = dP.EnemyTargetSelect(dP.enemy3MoveSelected, dP.enemy3Name);
                    }
                }
            }

            ChangeCharacterTurnDefensivePhase(); //If the characterTurn was "Slime1", call the function.
        }

        else if (characterTurn == "Slime2") //If it's Slime2's turn...
        {
            //If Slime2 is enemy1...
            if (dP.enemy1Name == "Slime2")
            {
                if (dP.enemy1MoveSelected == "Power-Up" && enemy1PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy1Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy1Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy1StatusEffectText, "STR +"));
                    enemy1STRBuff += 2;
                    enemy1PermSTRBuff += 2;
                    dP.enemy1MoveSelected = "";
                    dP.enemy1TargetSelected = "";
                }
                else if (dP.enemy1MoveSelected == "Power-Up" && enemy1PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy1TargetSelected = "";
                    dP.enemy1MoveSelected = "Attack";
                    while ((dP.enemy1TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy1TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy1TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy1TargetSelected == "")
                    {
                        dP.enemy1TargetSelected = dP.EnemyTargetSelect(dP.enemy1MoveSelected, dP.enemy1Name);
                    }
                }
            }
            //If Slime2 is enemy2...
            else if (dP.enemy2Name == "Slime2")
            {
                if (dP.enemy2MoveSelected == "Power-Up" && enemy2PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy2Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy2Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy2StatusEffectText, "STR +"));
                    enemy2STRBuff += 2;
                    enemy2PermSTRBuff += 2;
                    dP.enemy2MoveSelected = "";
                    dP.enemy2TargetSelected = "";
                }
                else if (dP.enemy2MoveSelected == "Power-Up" && enemy2PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy2TargetSelected = "";
                    dP.enemy2MoveSelected = "Attack";
                    while ((dP.enemy2TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy2TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy2TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy2TargetSelected == "")
                    {
                        dP.enemy2TargetSelected = dP.EnemyTargetSelect(dP.enemy2MoveSelected, dP.enemy2Name);
                    }
                }
            }
            //If Slime2 is enemy3...
            if (dP.enemy3Name == "Slime2")
            {
                if (dP.enemy3MoveSelected == "Power-Up" && enemy3PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy3Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy3Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy3StatusEffectText, "STR +"));
                    enemy3STRBuff += 2;
                    enemy3PermSTRBuff += 2;
                    dP.enemy3MoveSelected = "";
                    dP.enemy3TargetSelected = "";
                }
                else if (dP.enemy3MoveSelected == "Power-Up" && enemy3PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy3TargetSelected = "";
                    dP.enemy3MoveSelected = "Attack";
                    while ((dP.enemy3TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy3TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy3TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy3TargetSelected == "")
                    {
                        dP.enemy3TargetSelected = dP.EnemyTargetSelect(dP.enemy3MoveSelected, dP.enemy3Name);
                    }
                }
            }

            ChangeCharacterTurnDefensivePhase(); //If the characterTurn was "Slime2", call the function.
        }

        else if (characterTurn == "Slime3") //If it's Slime3's turn...
        {
            //If Slime3 is enemy1...
            if (dP.enemy1Name == "Slime3")
            {
                if (dP.enemy1MoveSelected == "Power-Up" && enemy1PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy1Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy1Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy1StatusEffectText, "STR +"));
                    enemy1STRBuff += 2;
                    enemy1PermSTRBuff += 2;
                    dP.enemy1MoveSelected = "";
                    dP.enemy1TargetSelected = "";
                }
                else if (dP.enemy1MoveSelected == "Power-Up" && enemy1PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy1TargetSelected = "";
                    dP.enemy1MoveSelected = "Attack";
                    while ((dP.enemy1TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy1TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy1TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy1TargetSelected == "")
                    {
                        dP.enemy1TargetSelected = dP.EnemyTargetSelect(dP.enemy1MoveSelected, dP.enemy1Name);
                    }
                }
            }
            //If Slime3 is enemy2...
            else if (dP.enemy2Name == "Slime3")
            {
                if (dP.enemy2MoveSelected == "Power-Up" && enemy2PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy2Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy2Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy2StatusEffectText, "STR +"));
                    enemy2STRBuff += 2;
                    enemy2PermSTRBuff += 2;
                    dP.enemy2MoveSelected = "";
                    dP.enemy2TargetSelected = "";
                }
                else if (dP.enemy2MoveSelected == "Power-Up" && enemy2PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy2TargetSelected = "";
                    dP.enemy2MoveSelected = "Attack";
                    while ((dP.enemy2TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy2TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy2TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy2TargetSelected == "")
                    {
                        dP.enemy2TargetSelected = dP.EnemyTargetSelect(dP.enemy2MoveSelected, dP.enemy2Name);
                    }
                }
            }
            //If Slime3 is enemy3...
            if (dP.enemy3Name == "Slime3")
            {
                if (dP.enemy3MoveSelected == "Power-Up" && enemy3PermSTRBuff < 6) //If the move selected is Power-Up and the character currently has less than 6 PERM STR...
                {
                    StartCoroutine(DoEnemy3Animation("BuffOrHealGiven"));
                    StartCoroutine(DoEnemy3Animation("Buffed"));
                    StartCoroutine(ShowPositiveStatusEffect(enemy3StatusEffectText, "STR +"));
                    enemy3STRBuff += 2;
                    enemy3PermSTRBuff += 2;
                    dP.enemy3MoveSelected = "";
                    dP.enemy3TargetSelected = "";
                }
                else if (dP.enemy3MoveSelected == "Power-Up" && enemy3PermSTRBuff >= 6) //If the move selected is Power-Up but the character has over 6 PERM STR...
                {
                    dP.enemy3TargetSelected = "";
                    dP.enemy3MoveSelected = "Attack";
                    while ((dP.enemy3TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy3TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy3TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy3TargetSelected == "")
                    {
                        dP.enemy3TargetSelected = dP.EnemyTargetSelect(dP.enemy3MoveSelected, dP.enemy3Name);
                    }
                }
            }

            ChangeCharacterTurnDefensivePhase(); //If the characterTurn was "Slime3", call the function.
        }
    }

    //Called when the characterTurn == "Slime1/Slime2/Slime3" and the move chosen was an offensive-type move.
    void SlimeOffensive()
    {
        if (characterTurn == "Slime1") //If it's Slime1's turn...
        {
            //If Slime1 is enemy1...
            if (dP.enemy1Name == "Slime1" && dP.enemy1MoveSelected != "")
            {
                StartCoroutine(DoEnemy1Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy1MoveSelected, dP.enemy1TargetSelected, enemy1STRBuff);
                dP.enemy1MoveSelected = "";
                dP.enemy1TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            //If Slime1 is enemy2...
            if (dP.enemy2Name == "Slime1" && dP.enemy2MoveSelected != "")
            {
                StartCoroutine(DoEnemy2Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy2MoveSelected, dP.enemy2TargetSelected, enemy2STRBuff);
                dP.enemy2MoveSelected = "";
                dP.enemy2TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            //If Slime1 is enemy3...
            if (dP.enemy3Name == "Slime1" && dP.enemy3MoveSelected != "")
            {
                StartCoroutine(DoEnemy3Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy3MoveSelected, dP.enemy3TargetSelected, enemy3STRBuff);
                dP.enemy3MoveSelected = "";
                dP.enemy3TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            else
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }

        if (characterTurn == "Slime2") //If it's Slime2's turn...
        {
            //If Slime2 is enemy1...
            if (dP.enemy1Name == "Slime2" && dP.enemy1MoveSelected != "")
            {
                StartCoroutine(DoEnemy1Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy1MoveSelected, dP.enemy1TargetSelected, enemy1STRBuff);
                dP.enemy1MoveSelected = "";
                dP.enemy1TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            //If Slime2 is enemy2...
            if (dP.enemy2Name == "Slime2" && dP.enemy2MoveSelected != "")
            {
                StartCoroutine(DoEnemy2Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy2MoveSelected, dP.enemy2TargetSelected, enemy2STRBuff);
                dP.enemy2MoveSelected = "";
                dP.enemy2TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            //If Slime2 is enemy3...
            if (dP.enemy3Name == "Slime2" && dP.enemy3MoveSelected != "")
            {
                StartCoroutine(DoEnemy3Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy3MoveSelected, dP.enemy3TargetSelected, enemy3STRBuff);
                dP.enemy3MoveSelected = "";
                dP.enemy3TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            else
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }

        if (characterTurn == "Slime3") //If it's Slime3's turn...
        {
            //If Slime3 is enemy1...
            if (dP.enemy1Name == "Slime3" && dP.enemy1MoveSelected != "")
            {
                StartCoroutine(DoEnemy1Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy1MoveSelected, dP.enemy1TargetSelected, enemy1STRBuff);
                dP.enemy1MoveSelected = "";
                dP.enemy1TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            //If Slime3 is enemy2...
            if (dP.enemy2Name == "Slime3" && dP.enemy2MoveSelected != "")
            {
                StartCoroutine(DoEnemy2Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy2MoveSelected, dP.enemy2TargetSelected, enemy2STRBuff);
                dP.enemy2MoveSelected = "";
                dP.enemy2TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            //If Slime3 is enemy3...
            if (dP.enemy3Name == "Slime3" && dP.enemy3MoveSelected != "")
            {
                StartCoroutine(DoEnemy3Animation("Attack"));
                SlimeOffensiveEnemyBranching(dP.enemy3MoveSelected, dP.enemy3TargetSelected, enemy3STRBuff);
                dP.enemy3MoveSelected = "";
                dP.enemy3TargetSelected = "";
                ChangeCharacterTurnOffensivePhase(false);
            }

            else
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }
    }

    void SlimeDefensiveEnemyBranching(string MoveSelected, string TargetSelected) //Used for ally branching in the SlimeDefensive function.
    {
        //Currently not needed
    }

    void SlimeOffensiveEnemyBranching(string MoveSelected, string TargetSelected, int STRbuff) //Used for ally branching in the SlimeOffensive function.
    {
        if (MoveSelected == "Attack")  //If the move selected is Attack...
        {
            accuracy = Random.Range(1, 101);

            //If the target selected has a smoke bomb effect on them, up the max accuracy range to decrease the accuracy.
            if (TargetSelected == dP.ally1Name && ally1HasSmokeBomb == true || TargetSelected == dP.ally2Name && ally2HasSmokeBomb == true || TargetSelected == dP.ally3Name && ally3HasSmokeBomb == true)
            {
                accuracy = Random.Range(1, 121);
            }

            if (TargetSelected == dP.ally1Name && accuracy <= 75) //If the target selected is ally1 and the accuracy is 70 or below... 
            {
                StartCoroutine(DoAlly1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Squish");
                dP.ally1Health -= DealDamage((6 + STRbuff) - ally1DEFBuff);
                StartCoroutine(ShowDamageDealt(ally1HealthChangeText, ((6 + STRbuff) - ally1DEFBuff)));
            }
            else if (TargetSelected == dP.ally1Name && accuracy > 75) //If the target selected is ally1 and the accuracy is above 70... 
            {
                StartCoroutine(ShowMiss(ally1MissText));
            }
            else if (TargetSelected == dP.ally2Name && accuracy <= 75) //If the target selected is ally2 and the accuracy is 70 or below... 
            {
                StartCoroutine(DoAlly2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Squish");
                dP.ally2Health -= DealDamage((6 + STRbuff) - ally2DEFBuff);
                StartCoroutine(ShowDamageDealt(ally2HealthChangeText, ((6 + STRbuff) - ally2DEFBuff)));
            }
            else if (TargetSelected == dP.ally2Name && accuracy > 75) //If the target selected is ally2 and the accuracy is above 70... 
            {
                StartCoroutine(ShowMiss(ally2MissText));
            }
            else if (TargetSelected == dP.ally3Name && accuracy <= 75) //If the target selected is ally3 and the accuracy is 70 or below... 
            {
                StartCoroutine(DoAlly3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Squish");
                dP.ally3Health -= DealDamage((6 + STRbuff) - ally3DEFBuff);
                StartCoroutine(ShowDamageDealt(ally3HealthChangeText, ((6 + STRbuff) - ally3DEFBuff)));
            }
            else if (TargetSelected == dP.ally3Name && accuracy > 75) //If the target selected is ally3 and the accuracy is above 70... 
            {
                StartCoroutine(ShowMiss(ally3MissText));
            }
        }
    }

    //Called when the characterTurn == "SlimeKing" and the move chosen was an defensive-type move.
    void SlimeKingDefensive()
    {
        if (characterTurn == "SlimeKing")
        {
            //If SlimeKing is Enemy1...
            if (dP.enemy1Name == "SlimeKing" && enemy1PermSTRBuff < 8) 
            {
                if (dP.enemy1MoveSelected == "Regenerate") //If the move selected is Regenerate and the character currently has less than 8 PERM STR...
                {
                    StartCoroutine(DoEnemy1Animation("Buffed"));
                    StartCoroutine(DoEnemy1Animation("BuffOrHealGiven"));
                    enemy1STRBuff += 2;
                    enemy1PermSTRBuff += 2;
                    dP.enemy1Health += HealHealth(10);
                    StartCoroutine(ShowPositiveStatusEffect(enemy1StatusEffectText, "STR +"));
                    StartCoroutine(ShowHealingDealt(enemy1HealthChangeText, 10));
                    dP.enemy1MoveSelected = "";
                    dP.enemy1TargetSelected = "";
                }
            }
            else if (dP.enemy1MoveSelected == "Regenerate" && enemy1PermSTRBuff >= 8) //If the move selected is Regenerate and the character currently has 8 or more PERM STR...
            {
                dP.enemy1TargetSelected = "";
                while (dP.enemy1MoveSelected == "Regenerate")
                {
                    dP.enemyMoveSelectNumber = Random.Range(1, 4); //Choose a random number between 1-3.

                    if (dP.enemyMoveSelectNumber == 1 || dP.enemyMoveSelectNumber == 2)
                    {
                        dP.enemy1MoveSelected = "Attack-All";
                    }
                    else if (dP.enemyMoveSelectNumber == 3)
                    {
                        dP.enemy1MoveSelected = "Attack";
                    }

                    while ((dP.enemy1TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy1TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy1TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy1TargetSelected == "")
                    {
                        dP.enemy1TargetSelected = dP.EnemyTargetSelect(dP.enemy1MoveSelected, dP.enemy1Name);
                    }
                }
            }

            //If SlimeKing is Enemy2...
            if (dP.enemy2Name == "SlimeKing")
            {
                if (dP.enemy2MoveSelected == "Regenerate" && enemy2PermSTRBuff < 8) //If the move selected is Regenerate and the character currently has less than 8 PERM STR...
                {
                    StartCoroutine(DoEnemy2Animation("Buffed"));
                    StartCoroutine(DoEnemy2Animation("BuffOrHealGiven"));
                    enemy2STRBuff += 2;
                    enemy2PermSTRBuff += 2;
                    dP.enemy2Health += HealHealth(10);
                    StartCoroutine(ShowPositiveStatusEffect(enemy2StatusEffectText, "STR +"));
                    StartCoroutine(ShowHealingDealt(enemy2HealthChangeText, 10));
                    dP.enemy2MoveSelected = "";
                    dP.enemy2TargetSelected = "";
                }
                else if (dP.enemy2MoveSelected == "Regenerate" && enemy2PermSTRBuff >= 8) //If the move selected is Regenerate and the character currently has 8 or more PERM STR...
                {
                    dP.enemy2TargetSelected = "";
                    while (dP.enemy2MoveSelected == "Regenerate")
                    {
                        dP.enemyMoveSelectNumber = Random.Range(1, 4); //Choose a random number between 1-3.

                        if (dP.enemyMoveSelectNumber == 1 || dP.enemyMoveSelectNumber == 2)
                        {
                            dP.enemy2MoveSelected = "Attack-All";
                        }
                        else if (dP.enemyMoveSelectNumber == 3)
                        {
                            dP.enemy2MoveSelected = "Attack";
                        }

                        while ((dP.enemy2TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy2TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy2TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy2TargetSelected == "")
                        {
                            dP.enemy2TargetSelected = dP.EnemyTargetSelect(dP.enemy2MoveSelected, dP.enemy2Name);
                        }
                    }
                }
            }

            //If SlimeKing is Enemy3...
            if (dP.enemy3Name == "SlimeKing")
            {
                if (dP.enemy3MoveSelected == "Regenerate" && enemy3PermSTRBuff < 8) //If the move selected is Regenerate and the character currently has less than 8 PERM STR...
                {
                    StartCoroutine(DoEnemy3Animation("Buffed"));
                    StartCoroutine(DoEnemy3Animation("BuffOrHealGiven"));
                    enemy3STRBuff += 2;
                    enemy3PermSTRBuff += 2;
                    dP.enemy3Health += HealHealth(10);
                    StartCoroutine(ShowPositiveStatusEffect(enemy3StatusEffectText, "STR +"));
                    StartCoroutine(ShowHealingDealt(enemy3HealthChangeText, 10));
                    dP.enemy3MoveSelected = "";
                    dP.enemy3TargetSelected = "";
                }
                else if (dP.enemy3MoveSelected == "Regenerate" && enemy3PermSTRBuff >= 8) //If the move selected is Regenerate and the character currently has 8 or more PERM STR...
                {
                    dP.enemy3TargetSelected = "";
                    while (dP.enemy3MoveSelected == "Regenerate")
                    {
                        dP.enemyMoveSelectNumber = Random.Range(1, 4); //Choose a random number between 1-3.

                        if (dP.enemyMoveSelectNumber == 1 || dP.enemyMoveSelectNumber == 2)
                        {
                            dP.enemy3MoveSelected = "Attack-All";
                        }
                        else if (dP.enemyMoveSelectNumber == 3)
                        {
                            dP.enemy3MoveSelected = "Attack";
                        }

                        while ((dP.enemy3TargetSelected == dP.ally1Name && dP.ally1Dead == true) || (dP.enemy3TargetSelected == dP.ally2Name && dP.ally2Dead == true) || (dP.enemy3TargetSelected == dP.ally3Name && dP.ally3Dead == true) || dP.enemy3TargetSelected == "")
                        {
                            dP.enemy3TargetSelected = dP.EnemyTargetSelect(dP.enemy3MoveSelected, dP.enemy3Name);
                        }
                    }
                }
            }

            ChangeCharacterTurnDefensivePhase(); //If the characterTurn was "SlimeKing", call the function.
        }
    }

    //Called when the characterTurn == "SlimeKing" and the move chosen was an defensive-type move.
    void SlimeKingOffensive()
    {
        if (characterTurn == "SlimeKing")
        {
            //If SlimeKing is enemy1...
            if (dP.enemy1Name == "SlimeKing" && dP.enemy1MoveSelected != "")
            {
                StartCoroutine(DoEnemy1Animation("Attack"));
                SlimeKingOffensiveEnemyBranching(dP.enemy1MoveSelected, dP.enemy1TargetSelected, enemy1STRBuff);
                dP.enemy1MoveSelected = "";
                dP.enemy1TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            //If SlimeKing is enemy2...
            else if (dP.enemy2Name == "SlimeKing" && dP.enemy2MoveSelected != "")
            {
                StartCoroutine(DoEnemy2Animation("Attack"));
                SlimeKingOffensiveEnemyBranching(dP.enemy2MoveSelected, dP.enemy2TargetSelected, enemy2STRBuff);
                dP.enemy2MoveSelected = "";
                dP.enemy2TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            //If SlimeKing is enemy3...
            else if (dP.enemy3Name == "SlimeKing" && dP.enemy3MoveSelected != "")
            {
                StartCoroutine(DoEnemy3Animation("Attack"));
                SlimeKingOffensiveEnemyBranching(dP.enemy3MoveSelected, dP.enemy3TargetSelected, enemy3STRBuff);
                dP.enemy3MoveSelected = "";
                dP.enemy3TargetSelected = "";
                ChangeCharacterTurnOffensivePhase();
            }

            else
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }
    }

    void SlimeKingDefensiveEnemyBranching(string MoveSelected, string TargetSelected) //Used for enemy branching in the SlimeKingDefensive function.
    {
        //Currently not needed.
    }

    void SlimeKingOffensiveEnemyBranching(string MoveSelected, string TargetSelected, int STRbuff) //Used for enemy branching in the SlimeKingOffensive function.
    {
        if (MoveSelected == "Attack") //If the move selected is Attack...
        {
            accuracy = Random.Range(1, 101);

            //If the target selected has a smoke bomb effect on them, up the max accuracy range to decrease the accuracy.
            if (TargetSelected == dP.ally1Name && ally1HasSmokeBomb == true || TargetSelected == dP.ally2Name && ally2HasSmokeBomb == true || TargetSelected == dP.ally3Name && ally3HasSmokeBomb == true)
            {
                accuracy = Random.Range(1, 121);
            }

            if (TargetSelected == dP.ally1Name && accuracy <= 80) //If the target selected is ally1 and the accuracy is 80 or below...
            {
                StartCoroutine(DoAlly1Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Squish");
                dP.ally1Health -= DealDamage((11 + STRbuff) - ally1DEFBuff);
                StartCoroutine(ShowDamageDealt(ally1HealthChangeText, ((11 + STRbuff) - ally1DEFBuff)));
            }
            else if (TargetSelected == dP.ally1Name && accuracy > 80) //If the target selected is ally1 but the attack misses...
            {
                StartCoroutine(ShowMiss(ally1MissText));
            }
            else if (TargetSelected == dP.ally2Name && accuracy <= 80) //If the target selected is ally2 and the accuracy is 80 or below...
            {
                StartCoroutine(DoAlly2Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Squish");
                dP.ally2Health -= DealDamage((11 + STRbuff) - ally2DEFBuff);
                StartCoroutine(ShowDamageDealt(ally2HealthChangeText, ((11 + STRbuff) - ally2DEFBuff)));
            }
            else if (TargetSelected == dP.ally2Name && accuracy > 80) //If the target selected is ally2 but the attack misses...
            {
                StartCoroutine(ShowMiss(ally2MissText));
            }
            else if (TargetSelected == dP.ally3Name && accuracy <= 80) //If the target selected is ally3 and the accuracy is 80 or below...
            {
                StartCoroutine(DoAlly3Animation("Injured"));
                FindObjectOfType<AudioManager>().Play("Squish");
                dP.ally3Health -= DealDamage((11 + STRbuff) - ally3DEFBuff);
                StartCoroutine(ShowDamageDealt(ally3HealthChangeText, ((11 + STRbuff) - ally3DEFBuff)));
            }
            else if (TargetSelected == dP.ally3Name && accuracy > 80) //If the target selected is ally3 but the attack misses...
            {
                StartCoroutine(ShowMiss(ally3MissText));
            }
        }

        else if (MoveSelected == "Attack-All") //If the move selected is Attack-All...
        {
            //Ally1
            accuracy = Random.Range(1, 101);

            //If the target selected has a smoke bomb effect on them, up the max accuracy range to decrease the accuracy.
            if (TargetSelected == dP.ally1Name && ally1HasSmokeBomb == true || TargetSelected == dP.ally2Name && ally2HasSmokeBomb == true || TargetSelected == dP.ally3Name && ally3HasSmokeBomb == true)
            {
                accuracy = Random.Range(1, 121);
            }

            if (dP.ally1Dead != true)
            {
                if (accuracy <= 80)
                {
                    StartCoroutine(DoAlly1Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("Squish");
                    dP.ally1Health -= DealDamage((6 + STRbuff) - ally1DEFBuff);
                    StartCoroutine(ShowDamageDealt(ally1HealthChangeText, ((6 + STRbuff) - ally1DEFBuff)));
                }
                else if (accuracy > 80)
                {
                    StartCoroutine(ShowMiss(ally1MissText));
                }
            }



            //Ally2
            accuracy = Random.Range(1, 101);

            //If the target selected has a smoke bomb effect on them, up the max accuracy range to decrease the accuracy.
            if (TargetSelected == dP.ally1Name && ally1HasSmokeBomb == true || TargetSelected == dP.ally2Name && ally2HasSmokeBomb == true || TargetSelected == dP.ally3Name && ally3HasSmokeBomb == true)
            {
                accuracy = Random.Range(1, 121);
            }

            if (dP.ally2Dead != true)
            {
                if (accuracy <= 80)
                {
                    StartCoroutine(DoAlly2Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("Squish");
                    dP.ally2Health -= DealDamage((6 + STRbuff) - ally2DEFBuff);
                    StartCoroutine(ShowDamageDealt(ally2HealthChangeText, ((6 + STRbuff) - ally2DEFBuff)));
                }
                else if (accuracy > 80)
                {
                    StartCoroutine(ShowMiss(ally2MissText));
                }
            }



            //Ally3
            accuracy = Random.Range(1, 101);

            //If the target selected has a smoke bomb effect on them, up the max accuracy range to decrease the accuracy.
            if (TargetSelected == dP.ally1Name && ally1HasSmokeBomb == true || TargetSelected == dP.ally2Name && ally2HasSmokeBomb == true || TargetSelected == dP.ally3Name && ally3HasSmokeBomb == true)
            {
                accuracy = Random.Range(1, 121);
            }

            if (dP.ally3Dead != true)
            {
                if (accuracy <= 80)
                {
                    StartCoroutine(DoAlly3Animation("Injured"));
                    FindObjectOfType<AudioManager>().Play("Squish");
                    dP.ally3Health -= DealDamage((6 + STRbuff) - ally3DEFBuff);
                    StartCoroutine(ShowDamageDealt(ally3HealthChangeText, ((6 + STRbuff) - ally3DEFBuff)));
                }
                else if (accuracy > 80)
                {
                    StartCoroutine(ShowMiss(ally3MissText));
                }
            }
        }
    }

    void Bleed() //Called in Update() if someone is bleeding; causes character to bleed for 2 rounds if active.
    {
        if (characterTurn == "Bleed")
        {
            if (someoneIsBleeding == true)
            {
                if (ally1IsBleeding == 1 && ally1HasBled == false) //If it's ally1's turn and their turns bleeding is set to 1...
                {
                    if (ally1DEFBuff > 4)
                    {
                        StartCoroutine(DoAlly1Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(ally1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally1HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoAlly1Animation("Injured"));
                        dP.ally1Health -= (4 - ally1DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(ally1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally1HealthChangeText, (4 - ally1DEFBuff)));
                    }
                    ally1IsBleeding = 2;
                    ally1HasBled = true;
                }
                else if (ally1IsBleeding == 2 && ally1HasBled == false) //If it's ally1's turn and their turns bleeding is set to 2...
                {
                    if (ally1DEFBuff > 4)
                    {
                        StartCoroutine(DoAlly1Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(ally1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally1HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoAlly1Animation("Injured"));
                        dP.ally1Health -= (4 - ally1DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(ally1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally1HealthChangeText, (4 - ally1DEFBuff)));
                    }
                    ally1IsBleeding = 0;
                    ally1HasBled = true;
                }

                if (ally2IsBleeding == 1 && ally2HasBled == false) //If it's ally2's turn and their turns bleeding is set to 1...
                {
                    if (ally2DEFBuff > 4)
                    {
                        StartCoroutine(DoAlly2Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(ally2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally2HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoAlly2Animation("Injured"));
                        dP.ally2Health -= (4 - ally2DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(ally2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally2HealthChangeText, (4 - ally2DEFBuff)));
                    }
                    ally2IsBleeding = 2;
                    ally2HasBled = true;
                }
                else if (ally2IsBleeding == 2 && ally2HasBled == false) //If it's ally2's turn and their turns bleeding is set to 2...
                {
                    if (ally2DEFBuff > 4)
                    {
                        StartCoroutine(DoAlly2Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(ally2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally2HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoAlly2Animation("Injured"));
                        dP.ally2Health -= (4 - ally2DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(ally2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally2HealthChangeText, (4 - ally2DEFBuff)));
                    }
                    ally2IsBleeding = 0;
                    ally2HasBled = true;
                }

                if (ally3IsBleeding == 1 && ally3HasBled == false) //If it's ally3's turn and their turns bleeding is set to 1...
                {
                    if (ally3DEFBuff > 4)
                    {
                        StartCoroutine(DoAlly3Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(ally3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally3HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoAlly3Animation("Injured"));
                        dP.ally3Health -= (4 - ally3DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(ally3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally3HealthChangeText, (4 - ally3DEFBuff)));
                    }
                    ally3IsBleeding = 2;
                    ally3HasBled = true;
                }
                else if (ally3IsBleeding == 2 && ally3HasBled == false) //If it's ally3's turn and their turns bleeding is set to 2...
                {
                    if (ally3DEFBuff > 4)
                    {
                        StartCoroutine(DoAlly3Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(ally3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally3HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoAlly3Animation("Injured"));
                        dP.ally3Health -= (4 - ally3DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(ally3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(ally3HealthChangeText, (4 - ally3DEFBuff)));
                    }
                    ally3IsBleeding = 0;
                    ally3HasBled = true;
                }

                if (enemy1IsBleeding == 1 && enemy1HasBled == false) //If it's enemy1's turn and their turns bleeding is set to 1...
                {
                    if (enemy1DEFBuff > 4)
                    {
                        StartCoroutine(DoEnemy1Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoEnemy1Animation("Injured"));
                        dP.enemy1Health -= (4 - enemy1DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, (4 - enemy1DEFBuff)));
                    }
                    enemy1IsBleeding = 2;
                    enemy1HasBled = true;
                }
                else if (enemy1IsBleeding == 2 && enemy1HasBled == false) //If it's enemy1's turn and their turns bleeding is set to 2...
                {
                    if (enemy1DEFBuff > 4)
                    {
                        StartCoroutine(DoEnemy1Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoEnemy1Animation("Injured"));
                        dP.enemy1Health -= (4 - enemy1DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(enemy1StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy1HealthChangeText, (4 - enemy1DEFBuff)));
                    }
                    enemy1IsBleeding = 0;
                    enemy1HasBled = true;
                }

                if (enemy2IsBleeding == 1 && enemy2HasBled == false) //If it's enemy2's turn and their turns bleeding is set to 1...
                {
                    if (enemy2DEFBuff > 4)
                    {
                        StartCoroutine(DoEnemy2Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoEnemy2Animation("Injured"));
                        dP.enemy2Health -= (4 - enemy2DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, (4 - enemy2DEFBuff)));
                    }
                    enemy2IsBleeding = 2;
                    enemy2HasBled = true;
                }
                else if (enemy2IsBleeding == 2 && enemy2HasBled == false) //If it's enemy2's turn and their turns bleeding is set to 2...
                {
                    if (enemy2DEFBuff > 4)
                    {
                        StartCoroutine(DoEnemy2Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoEnemy2Animation("Injured"));
                        dP.enemy2Health -= (4 - enemy2DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(enemy2StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy2HealthChangeText, (4 - enemy2DEFBuff)));
                    }
                    enemy2IsBleeding = 0;
                    enemy2HasBled = true;
                }

                if (enemy3IsBleeding == 1 && enemy3HasBled == false) //If it's enemy3's turn and their turns bleeding is set to 1...
                {
                    if (enemy3DEFBuff > 4)
                    {
                        StartCoroutine(DoEnemy3Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoEnemy3Animation("Injured"));
                        dP.enemy3Health -= (4 - enemy3DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, (4 - enemy3DEFBuff)));
                    }
                    enemy3IsBleeding = 2;
                    enemy3HasBled = true;
                }
                else if (enemy3IsBleeding == 2 && enemy3HasBled == false) //If it's enemy3's turn and their turns bleeding is set to 2...
                {
                    if (enemy3DEFBuff > 4)
                    {
                        StartCoroutine(DoEnemy3Animation("Injured"));
                        StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, 0));
                    }
                    else
                    {
                        StartCoroutine(DoEnemy3Animation("Injured"));
                        dP.enemy3Health -= (4 - enemy3DEFBuff);
                        StartCoroutine(ShowNegativeStatusEffect(enemy3StatusEffectText, "BLEEDING"));
                        StartCoroutine(ShowDamageDealt(enemy3HealthChangeText, (4 - enemy3DEFBuff)));
                    }
                    enemy3IsBleeding = 0;
                    enemy3HasBled = true;
                }

                ChangeCharacterTurnOffensivePhase();
            }
            else if (someoneIsBleeding == false)
            {
                ChangeCharacterTurnOffensivePhase(true);
            }
        }
    }

    void CancelTurnIfDead() //Called each frame in Update() during OffensivePhase. If an enemy dies during OffensivePhase, cancel the enemy's attack. If a target's enemy dies during OffensivePhase, cancel the target's attack.
    {
        // If ally1's target is dead, cancel their attack.
        if (dP.ally1TargetSelected == dP.enemy1Name && dP.enemy1Dead == true || dP.ally1TargetSelected == dP.enemy2Name && dP.enemy2Dead == true || dP.ally1TargetSelected == dP.enemy3Name && dP.enemy3Dead == true)
        {
            dP.ally1MoveSelected = "";
            dP.ally1TargetSelected = "";
            ally1IsCharging = false;
        }

        // If ally2's target is dead, cancel their attack.
        if (dP.ally2TargetSelected == dP.enemy1Name && dP.enemy1Dead == true || dP.ally2TargetSelected == dP.enemy2Name && dP.enemy2Dead == true || dP.ally2TargetSelected == dP.enemy3Name && dP.enemy3Dead == true)
        {
            dP.ally2MoveSelected = "";
            dP.ally2TargetSelected = "";
            ally2IsCharging = false;
        }

        // If ally3's target is dead, cancel their attack.
        if (dP.ally3TargetSelected == dP.enemy1Name && dP.enemy1Dead == true || dP.ally3TargetSelected == dP.enemy2Name && dP.enemy2Dead == true || dP.ally3TargetSelected == dP.enemy3Name && dP.enemy3Dead == true)
        {
            dP.ally3MoveSelected = "";
            dP.ally3TargetSelected = "";
            ally3IsCharging = false;
        }

        // If enemy1's target is dead, cancel their attack.
        if (dP.enemy1TargetSelected == dP.ally1Name && dP.ally1Dead == true || dP.enemy1TargetSelected == dP.ally2Name && dP.ally2Dead == true || dP.enemy1TargetSelected == dP.ally3Name && dP.ally3Dead == true)
        {
            dP.enemy1MoveSelected = "";
            dP.enemy1TargetSelected = "";
            enemy1IsCharging = false;
        }

        // If enemy2's target is dead, cancel their attack.
        if (dP.enemy2TargetSelected == dP.ally1Name && dP.ally1Dead == true || dP.enemy2TargetSelected == dP.ally2Name && dP.ally2Dead == true || dP.enemy2TargetSelected == dP.ally3Name && dP.ally3Dead == true)
        {
            dP.enemy2MoveSelected = "";
            dP.enemy2TargetSelected = "";
            enemy2IsCharging = false;
        }

        // If enemy3's target is dead, cancel their attack.
        if (dP.enemy3TargetSelected == dP.ally1Name && dP.ally1Dead == true || dP.enemy3TargetSelected == dP.ally2Name && dP.ally2Dead == true || dP.enemy3TargetSelected == dP.ally3Name && dP.ally3Dead == true)
        {
            dP.enemy3MoveSelected = "";
            dP.enemy3TargetSelected = "";
            enemy3IsCharging = false;
        }

        // If enemy1 is dead, cancel their attack.
        if (dP.enemy1Dead == true)
        {
            dP.enemy1MoveSelected = "";
            dP.enemy1TargetSelected = "";
        }
        // If enemy2 is dead, cancel their attack.
        if (dP.enemy2Dead == true)
        {
            dP.enemy2MoveSelected = "";
            dP.enemy2TargetSelected = "";
        }
        // If enemy3 is dead, cancel their attack.
        if (dP.enemy3Dead == true)
        {
            dP.enemy3MoveSelected = "";
            dP.enemy3TargetSelected = "";
        }
    }

    void ChangeCharacterTurnDefensivePhase() //Called when it's needed to change characterTurn during the DefensivePhase;  after enemy3's defensive phase turn is over set the defensivePhase bool to false.
    {
        //If the current turn is ally1...
        if (characterTurn == dP.ally1Name && dP.ally1MoveSelected == "" && dP.ally1Dead == false) //If ally1 made a move during this phase...
        {
            StartCoroutine(CCTDPDowntime(dP.ally2Name));
        }
        else if (characterTurn == dP.ally1Name && dP.ally1MoveSelected != "" || characterTurn == dP.ally1Name && dP.ally1Dead == true) //If ally1 hasn't made a move during this phase or is dead...
        {
            characterTurn = dP.ally2Name;
        }

        //If the current turn is ally2...
        else if (characterTurn == dP.ally2Name && dP.ally2MoveSelected == "" && dP.ally2Dead == false) //If ally2 made a move during this phase...
        {
            StartCoroutine(CCTDPDowntime(dP.ally3Name));
        }
        else if (characterTurn == dP.ally2Name && dP.ally2MoveSelected != "" || characterTurn == dP.ally2Name && dP.ally2Dead == true) //If ally2 hasn't made a move during this phase or is dead...
        {
            characterTurn = dP.ally3Name;
        }

        //If the current turn is ally3...
        else if (characterTurn == dP.ally3Name && dP.ally3MoveSelected == "" && dP.ally3Dead == false) //If ally3 made a move during this phase...
        {
            StartCoroutine(CCTDPDowntime(dP.enemy1Name));
        }
        else if (characterTurn == dP.ally3Name && dP.ally3MoveSelected != "" || characterTurn == dP.ally3Name && dP.ally3Dead == true) //If ally3 hasn't made a move during this phase or is dead...
        {
            characterTurn = dP.enemy1Name;
        }

        //If the current turn is enemy1...
        else if (characterTurn == dP.enemy1Name && dP.enemy1MoveSelected == "" && dP.enemy1Dead == false) //If enemy1 made a move during this phase...
        {
            StartCoroutine(CCTDPDowntime(dP.enemy2Name));
        }
        else if (characterTurn == dP.enemy1Name && dP.enemy1MoveSelected != "" || characterTurn == dP.enemy1Name && dP.enemy1Dead == true) //If enemy1 hasn't made a move during this phase or is dead...
        {
            characterTurn = dP.enemy2Name;
        }

        //If the current turn is enemy2...
        else if (characterTurn == dP.enemy2Name && dP.enemy2MoveSelected == "" && dP.enemy2Dead == false) //If enemy2 made a move during this phase...
        {
            StartCoroutine(CCTDPDowntime(dP.enemy3Name));
        }
        else if (characterTurn == dP.enemy2Name && dP.enemy2MoveSelected != "" || characterTurn == dP.enemy2Name && dP.enemy2Dead == true) //If enemy2 hasn't made a move during this phase or is dead...
        {
            characterTurn = dP.enemy3Name;
        }

        //If the current turn is enemy3
        else if (characterTurn == dP.enemy3Name && dP.enemy3MoveSelected == "" && dP.enemy3Dead == false) //If enemy3 made a move during this phase...
        {
            StartCoroutine(CCTDPDowntime(dP.ally1Name, defensivePhase = false));
        }
        else if (characterTurn == dP.enemy3Name && dP.enemy3MoveSelected != "" || characterTurn == dP.enemy3Name && dP.enemy3Dead == true) //If enemy3 hasn't made a move during this phase or is dead...
        {
            characterTurn = dP.ally1Name;
            defensivePhase = false;
        }
    }

    IEnumerator CCTDPDowntime(string NextCharacterName, bool DefensivePhase = true) //To be called everytime the character turn in the DefensivePhase is to be changed after a character made a move. Waits 5 seconds before each change.
    {
        characterTurn = "";
        yield return new WaitForSeconds(3);
        characterTurn = NextCharacterName;
        defensivePhase = DefensivePhase;      
    }

    void ChangeCharacterTurnOffensivePhase(bool Skip = false) //Called when it's needed to change characterTurn during the OffensivePhase. After enemy3's turn is finished it'll turn off ActionPhase and set up variables for DecisionPhase.
    {
        //If the current turn is ally1...
        if (characterTurn == dP.ally1Name && Skip == false) //If ally1 made a move during this phase...
        {
            StartCoroutine(CCTOPDowntime(dP.ally2Name));
        }
        else if (characterTurn == dP.ally1Name && Skip == true) //If ally1 didn't made a move during this phase...
        {
            characterTurn = dP.ally2Name;
        }

        //If the current turn is ally2...
        else if (characterTurn == dP.ally2Name && Skip == false) //If ally2 made a move during this phase...
        {
            StartCoroutine(CCTOPDowntime(dP.ally3Name));
        }
        else if (characterTurn == dP.ally2Name && Skip == true) //If ally2 didn't made a move during this phase...
        {
            characterTurn = dP.ally3Name;
        }

        //If the current turn is ally3...
        else if (characterTurn == dP.ally3Name && Skip == false) //If ally3 made a move during this phase...
        {
            StartCoroutine(CCTOPDowntime(dP.enemy1Name));
        }
        else if (characterTurn == dP.ally3Name && Skip == true) //If ally3 didn't made a move during this phase...
        {
            characterTurn = dP.enemy1Name;          
        }

        //If the current turn is enemy1...
        else if (characterTurn == dP.enemy1Name && Skip == false) //If enemy1 made a move during this phase...
        {
            StartCoroutine(CCTOPDowntime(dP.enemy2Name));
        }
        else if (characterTurn == dP.enemy1Name && Skip == true) //If enemy1 didn't made a move during this phase...
        {
            characterTurn = dP.enemy2Name;
        }

        //If the current turn is enemy2...
        else if (characterTurn == dP.enemy2Name && Skip == false) //If enemy2 made a move during this phase...
        {
            StartCoroutine(CCTOPDowntime(dP.enemy3Name));
        }
        else if (characterTurn == dP.enemy2Name && Skip == true) //If enemy2 didn't made a move during this phase...
        {
            characterTurn = dP.enemy3Name;
        }

        //If the current turn is enemy3...
        else if (characterTurn == dP.enemy3Name && Skip == false) //If enemy3 made a move during this phase...
        {
            StartCoroutine(CCTOPDowntime("Bleed"));
        }
        else if (characterTurn == dP.enemy3Name && Skip == true) //If enemy3 didn't made a move during this phase...
        {
            characterTurn = "Bleed";
        }

        //If the current turn is for Bleeding Effects...
        else if (characterTurn == "Bleed" && Skip == false) //If bleed was active during this phase...
        {
            StartCoroutine(CCTOPDowntimeFinal());
        }
        else if (characterTurn == "Bleed" && Skip == true) //If bleed wasn't active during this phase...
        {
            characterTurn = "";
            dP.actionPhase = false;

            //dP.ally1MoveSelected = ""; dP.ally2MoveSelected = ""; dP.ally3MoveSelected = ""; dP.enemy1MoveSelected = ""; dP.enemy2MoveSelected = ""; dP.enemy3MoveSelected = "";
            //dP.ally1TargetSelected = ""; dP.ally2TargetSelected = ""; dP.ally3TargetSelected = ""; dP.enemy1TargetSelected = ""; dP.enemy2TargetSelected = ""; dP.enemy3TargetSelected = "";

            ally1STRBuff = ally1PermSTRBuff;
            ally2STRBuff = ally2PermSTRBuff;
            ally3STRBuff = ally3PermSTRBuff;
            enemy1STRBuff = enemy1PermSTRBuff;
            enemy2STRBuff = enemy2PermSTRBuff;
            enemy3STRBuff = enemy3PermSTRBuff;

            ally1DEFBuff = ally1PermDEFBuff;
            ally2DEFBuff = ally2PermDEFBuff;
            ally3DEFBuff = ally3PermDEFBuff;
            enemy1DEFBuff = enemy1PermDEFBuff;
            enemy2DEFBuff = enemy2PermDEFBuff;
            enemy3DEFBuff = enemy3PermDEFBuff;

            dP.ally1Move1.SetActive(true);
            dP.ally1Move2.SetActive(true);
            dP.ally1Move3.SetActive(true);
            dP.ally1Move4.SetActive(true);
            dP.defendMove.SetActive(true);

            dP.Move1Button.SetActive(true);
            dP.Move2Button.SetActive(true);
            dP.Move3Button.SetActive(true);
            dP.Move4Button.SetActive(true);
            dP.DefendButton.SetActive(true);

            dP.characterTurn = dP.ally1Name;
            dP.charactersTurnText.text = dP.characterTurn + "'s Turn";

            ally1HasBled = false;
            ally2HasBled = false;
            ally3HasBled = false;
            enemy1HasBled = false;
            enemy2HasBled = false;
            enemy3HasBled = false;
        }
    }

    IEnumerator CCTOPDowntime(string NextCharacterName) //To be called everytime the character turn in the OffensivePhase is to be changed after a character made a move. Waits 5 seconds before each change.
    {
        characterTurn = "";
        yield return new WaitForSeconds(3);
        characterTurn = NextCharacterName;
    }

    IEnumerator CCTOPDowntimeFinal() //To be called after the final character turn in the OffensivePhase. Waits 5 seconds before each change.
    {
        characterTurn = "";
        yield return new WaitForSeconds(3);
        
        characterTurn = "";
        dP.actionPhase = false;

        //dP.ally1MoveSelected = ""; dP.ally2MoveSelected = ""; dP.ally3MoveSelected = ""; dP.enemy1MoveSelected = ""; dP.enemy2MoveSelected = ""; dP.enemy3MoveSelected = "";
        //dP.ally1TargetSelected = ""; dP.ally2TargetSelected = ""; dP.ally3TargetSelected = ""; dP.enemy1TargetSelected = ""; dP.enemy2TargetSelected = ""; dP.enemy3TargetSelected = "";

        ally1STRBuff = ally1PermSTRBuff;
        ally2STRBuff = ally2PermSTRBuff;
        ally3STRBuff = ally3PermSTRBuff;
        enemy1STRBuff = enemy1PermSTRBuff;
        enemy2STRBuff = enemy2PermSTRBuff;
        enemy3STRBuff = enemy3PermSTRBuff;

        ally1DEFBuff = ally1PermDEFBuff;
        ally2DEFBuff = ally2PermDEFBuff;
        ally3DEFBuff = ally3PermDEFBuff;
        enemy1DEFBuff = enemy1PermDEFBuff;
        enemy2DEFBuff = enemy2PermDEFBuff;
        enemy3DEFBuff = enemy3PermDEFBuff;

        dP.ally1Move1.SetActive(true);
        dP.ally1Move2.SetActive(true);
        dP.ally1Move3.SetActive(true);
        dP.ally1Move4.SetActive(true);
        dP.defendMove.SetActive(true);

        dP.Move1Button.SetActive(true);
        dP.Move2Button.SetActive(true);
        dP.Move3Button.SetActive(true);
        dP.Move4Button.SetActive(true);
        dP.DefendButton.SetActive(true);

        dP.characterTurn = dP.ally1Name;
        dP.charactersTurnText.text = dP.characterTurn + "'s Turn";

        ally1HasSmokeBomb = false;
        ally2HasSmokeBomb = false;
        ally3HasSmokeBomb = false;

        ally1HasBled = false;
        ally2HasBled = false;
        ally3HasBled = false;
        enemy1HasBled = false;
        enemy2HasBled = false;
        enemy3HasBled = false;
    }

    int DealDamage(int AmountLost) //Used to deal damage to an opponent. Ensures that if the amount dealt is lower than 0 it'll instead just deal 0 damage.
    {
        if (AmountLost >= 0)
        {
            return AmountLost;
        }
        else
        {
            return 0;
        }
    }

    int HealHealth(int AmountGained) //Used to heal an ally's health. Ensures that if the amount healed is lower than 0 it'll instead just heal for 0.
    {
        if (AmountGained >= 0)
        {
            return AmountGained;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator ShowNegativeStatusEffect(TextMeshProUGUI CharacterEffected, string StatusEffect) //When called, shows the negative inflicted status effect of the character for 3 seconds.
    {
        FindObjectOfType<AudioManager>().Play("PowerDown");
        CharacterEffected.color = new Color32(255, 30, 30, 200);
        CharacterEffected.text = StatusEffect;
        yield return new WaitForSeconds(2.5f);
        CharacterEffected.text = "";
    }

    IEnumerator ShowPositiveStatusEffect(TextMeshProUGUI CharacterEffected, string StatusEffect) //When called, shows the positive inflicted status effect of the character for 3 seconds.
    {
        FindObjectOfType<AudioManager>().Play("PowerUp");
        CharacterEffected.color = new Color32(30, 30, 255, 200);
        CharacterEffected.text = StatusEffect;
        yield return new WaitForSeconds(2.5f);
        CharacterEffected.text = "";
    }

    IEnumerator ShowDamageDealt(TextMeshProUGUI CharacterHealthLost, int AmountLost) //When called, shows the damage dealt to the character for 3 seconds.
    {
        if (AmountLost >= 0)
        {
            CharacterHealthLost.color = new Color32(255, 0, 0, 200);
            CharacterHealthLost.text = "- " + AmountLost;
            yield return new WaitForSeconds(2.5f);
            CharacterHealthLost.text = "";
        }
        else
        {           
            CharacterHealthLost.color = new Color32(255, 0, 0, 200);
            CharacterHealthLost.text = "- " + 0;
            yield return new WaitForSeconds(2.5f);
            CharacterHealthLost.text = "";
        }
    }

    IEnumerator ShowHealingDealt(TextMeshProUGUI CharacterHealthGained, int AmountGained) //When called, shows the heal dealt to the character for 3 seconds.
    {
        if (AmountGained >= 0)
        {
            FindObjectOfType<AudioManager>().Play("Heal");
            CharacterHealthGained.color = new Color32(0, 255, 0, 200);
            CharacterHealthGained.text = "+ " + AmountGained;
            yield return new WaitForSeconds(2.5f);
            CharacterHealthGained.text = "";
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Heal");
            CharacterHealthGained.color = new Color32(0, 255, 0, 200);
            CharacterHealthGained.text = "+ " + 0;
            yield return new WaitForSeconds(2.5f);
            CharacterHealthGained.text = "";
        }

    }

    IEnumerator ShowMiss(TextMeshProUGUI CharacterMissText) //When called, shows the damage dealt to the character for 3 seconds.
    {
        FindObjectOfType<AudioManager>().Play("Miss");
        CharacterMissText.text = "MISS";
        yield return new WaitForSeconds(2.5f);
        CharacterMissText.text = "";
    }

    public IEnumerator DoAlly1Animation(string Animation) //Is called when Ally1 needs to play an animation.
    {
        if (Animation == "Injured")
        {
            dP.ally1Anim.SetBool("CharacterInjured", true);
            yield return new WaitForSeconds(1);
            dP.ally1Anim.SetBool("CharacterInjured", false);
        }

        else if (Animation == "Healed")
        {
            dP.ally1Anim.SetBool("CharacterHealed", true);
            yield return new WaitForSeconds(1);
            dP.ally1Anim.SetBool("CharacterHealed", false);
        }

        else if (Animation == "Attack")
        {
            dP.ally1Anim.SetBool("CharacterAttacks", true);
            yield return new WaitForSeconds(1);
            dP.ally1Anim.SetBool("CharacterAttacks", false);
        }

        else if (Animation == "Buffed")
        {
            dP.ally1Anim.SetBool("CharacterBuffed", true);
            yield return new WaitForSeconds(1);
            dP.ally1Anim.SetBool("CharacterBuffed", false);
        }

        else if (Animation == "BuffOrHealGiven")
        {
            dP.ally1Anim.SetBool("CharacterGiveBuffOrHeal", true);
            yield return new WaitForSeconds(1);
            dP.ally1Anim.SetBool("CharacterGiveBuffOrHeal", false);
        }

        else if (Animation == "Death")
        {
            dP.ally1Anim.SetBool("CharacterKilled", true);
            yield return new WaitForSeconds(1);
            dP.ally1Sprite.sprite = null;
            dP.ally1Anim.SetBool("CharacterKilled", false);
        }
    }

    public IEnumerator DoAlly2Animation(string Animation) //Is called when Ally2 needs to play an animation.
    {
        if (Animation == "Injured")
        {
            dP.ally2Anim.SetBool("CharacterInjured", true);
            yield return new WaitForSeconds(1);
            dP.ally2Anim.SetBool("CharacterInjured", false);
        }

        else if (Animation == "Healed")
        {
            dP.ally2Anim.SetBool("CharacterHealed", true);
            yield return new WaitForSeconds(1);
            dP.ally2Anim.SetBool("CharacterHealed", false);
        }

        else if (Animation == "Attack")
        {
            dP.ally2Anim.SetBool("CharacterAttacks", true);
            yield return new WaitForSeconds(1);
            dP.ally2Anim.SetBool("CharacterAttacks", false);
        }

        else if (Animation == "Buffed")
        {
            dP.ally2Anim.SetBool("CharacterBuffed", true);
            yield return new WaitForSeconds(1);
            dP.ally2Anim.SetBool("CharacterBuffed", false);
        }

        else if (Animation == "BuffOrHealGiven")
        {
            dP.ally2Anim.SetBool("CharacterGiveBuffOrHeal", true);
            yield return new WaitForSeconds(1);
            dP.ally2Anim.SetBool("CharacterGiveBuffOrHeal", false);
        }

        else if (Animation == "Death")
        {
            dP.ally2Anim.SetBool("CharacterKilled", true);
            yield return new WaitForSeconds(1);
            dP.ally2Sprite.sprite = null;
            dP.ally2Anim.SetBool("CharacterKilled", false);
        }
    }

    public IEnumerator DoAlly3Animation(string Animation) //Is called when Ally3 needs to play an animation.
    {
        if (Animation == "Injured")
        {
            dP.ally3Anim.SetBool("CharacterInjured", true);
            yield return new WaitForSeconds(1);
            dP.ally3Anim.SetBool("CharacterInjured", false);
        }

        else if (Animation == "Healed")
        {
            dP.ally3Anim.SetBool("CharacterHealed", true);
            yield return new WaitForSeconds(1);
            dP.ally3Anim.SetBool("CharacterHealed", false);
        }

        else if (Animation == "Attack")
        {
            dP.ally3Anim.SetBool("CharacterAttacks", true);
            yield return new WaitForSeconds(1);
            dP.ally3Anim.SetBool("CharacterAttacks", false);
        }

        else if (Animation == "Buffed")
        {
            dP.ally3Anim.SetBool("CharacterBuffed", true);
            yield return new WaitForSeconds(1);
            dP.ally3Anim.SetBool("CharacterBuffed", false);
        }

        else if (Animation == "BuffOrHealGiven")
        {
            dP.ally3Anim.SetBool("CharacterGiveBuffOrHeal", true);
            yield return new WaitForSeconds(1);
            dP.ally3Anim.SetBool("CharacterGiveBuffOrHeal", false);
        }

        else if (Animation == "Death")
        {
            dP.ally3Anim.SetBool("CharacterKilled", true);
            yield return new WaitForSeconds(1);
            dP.ally3Sprite.sprite = null;
            dP.ally3Anim.SetBool("CharacterKilled", false);
        }
    }

    public IEnumerator DoEnemy1Animation(string Animation) //Is called when Enemy1 needs to play an animation.
    {
        if (Animation == "Injured")
        {
            dP.enemy1Anim.SetBool("CharacterInjured", true);
            yield return new WaitForSeconds(1);
            dP.enemy1Anim.SetBool("CharacterInjured", false);
        }

        else if (Animation == "Healed")
        {
            dP.enemy1Anim.SetBool("CharacterHealed", true);
            yield return new WaitForSeconds(1);
            dP.enemy1Anim.SetBool("CharacterHealed", false);
        }

        else if (Animation == "Attack")
        {
            dP.enemy1Anim.SetBool("CharacterAttacks", true);
            yield return new WaitForSeconds(1);
            dP.enemy1Anim.SetBool("CharacterAttacks", false);
        }

        else if (Animation == "Buffed")
        {
            dP.enemy1Anim.SetBool("CharacterBuffed", true);
            yield return new WaitForSeconds(1);
            dP.enemy1Anim.SetBool("CharacterBuffed", false);
        }

        else if (Animation == "BuffOrHealGiven")
        {
            dP.enemy1Anim.SetBool("CharacterGiveBuffOrHeal", true);
            yield return new WaitForSeconds(1);
            dP.enemy1Anim.SetBool("CharacterGiveBuffOrHeal", false);
        }

        else if (Animation == "Death")
        {
            dP.enemy1Anim.SetBool("CharacterKilled", true);
            yield return new WaitForSeconds(1);
            dP.enemy1Sprite.sprite = null;
            dP.enemy1Anim.SetBool("CharacterKilled", false);
        }
    }

    public IEnumerator DoEnemy2Animation(string Animation) //Is called when Enemy2 needs to play an animation.
    {
        if (Animation == "Injured")
        {
            dP.enemy2Anim.SetBool("CharacterInjured", true);
            yield return new WaitForSeconds(1);
            dP.enemy2Anim.SetBool("CharacterInjured", false);
        }

        else if (Animation == "Healed")
        {
            dP.enemy2Anim.SetBool("CharacterHealed", true);
            yield return new WaitForSeconds(1);
            dP.enemy2Anim.SetBool("CharacterHealed", false);
        }

        else if (Animation == "Attack")
        {
            dP.enemy2Anim.SetBool("CharacterAttacks", true);
            yield return new WaitForSeconds(1);
            dP.enemy2Anim.SetBool("CharacterAttacks", false);
        }

        else if (Animation == "Buffed")
        {
            dP.enemy2Anim.SetBool("CharacterBuffed", true);
            yield return new WaitForSeconds(1);
            dP.enemy2Anim.SetBool("CharacterBuffed", false);
        }

        else if (Animation == "BuffOrHealGiven")
        {
            dP.enemy2Anim.SetBool("CharacterGiveBuffOrHeal", true);
            yield return new WaitForSeconds(1);
            dP.enemy2Anim.SetBool("CharacterGiveBuffOrHeal", false);
        }

        else if (Animation == "Death")
        {
            dP.enemy2Anim.SetBool("CharacterKilled", true);
            yield return new WaitForSeconds(1);
            dP.enemy2Sprite.sprite = null;
            dP.enemy2Anim.SetBool("CharacterKilled", false);
        }
    }

    public IEnumerator DoEnemy3Animation(string Animation) //Is called when Enemy3 needs to play an animation.
    {
        if (Animation == "Injured")
        {
            dP.enemy3Anim.SetBool("CharacterInjured", true);
            yield return new WaitForSeconds(1);
            dP.enemy3Anim.SetBool("CharacterInjured", false);
        }

        else if (Animation == "Healed")
        {
            dP.enemy3Anim.SetBool("CharacterHealed", true);
            yield return new WaitForSeconds(1);
            dP.enemy3Anim.SetBool("CharacterHealed", false);
        }

        else if (Animation == "Attack")
        {
            dP.enemy3Anim.SetBool("CharacterAttacks", true);
            yield return new WaitForSeconds(1);
            dP.enemy3Anim.SetBool("CharacterAttacks", false);
        }

        else if (Animation == "Buffed")
        {
            dP.enemy3Anim.SetBool("CharacterBuffed", true);
            yield return new WaitForSeconds(1);
            dP.enemy3Anim.SetBool("CharacterBuffed", false);
        }

        else if (Animation == "BuffOrHealGiven")
        {
            dP.enemy3Anim.SetBool("CharacterGiveBuffOrHeal", true);
            yield return new WaitForSeconds(1);
            dP.enemy3Anim.SetBool("CharacterGiveBuffOrHeal", false);
        }

        else if (Animation == "Death")
        {
            dP.enemy3Anim.SetBool("CharacterKilled", true);
            yield return new WaitForSeconds(1);
            dP.enemy3Sprite.sprite = null;
            dP.enemy3Anim.SetBool("CharacterKilled", false);
        }
    }
}
