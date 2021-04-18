using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public KeyCode _Key;
    public KeyCode _AltKey;
    public Button _button;
    public Animator NotifAnimator;
    public Text notifText;
    public Text next;
 
    void Awake()
    {
        _button = GetComponent<Button>();
        switch (PlayerPrefs.GetString("lang"))
        {
            case "ru":
                notifText.text = "Нажмите Е, чтобы взаимодействовать";
                next.text = "Далее";
                break;
            case "eng":
                notifText.text = "Press E to interact";
                next.text = "Next";
                break;
        }
    }

    void Update ()
    {
        WhatSign();
        if (NotifAnimator.GetBool("NOpen") && Input.GetButtonDown("Submit"))
        {
            _button.onClick.Invoke();
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
 
    void WhatSign()
    {
        switch (PlayerPrefs.GetString("lang"))
        {
            case "ru":
                WhatSignRu();
                break;
            case "eng":
                WhatSignEng();
                break;
        }
    }

    void WhatSignRu()
    {
        switch (NotifAnimator.GetInteger("SignNum"))
        {
            case 1:
                dialogue.name = "Обучение";
                dialogue.sentences = new string[] { "Добро пожаловать в игру!",
                                                    "Для передвижения можно использовать стрелки ← / → , клавиши A / D, а также левый стик геймпада.",
                                                    "Попробуй добежать до следующей таблички, используя новые знания!"};
                break;
            case 2:
                dialogue.name = "Обучение";
                dialogue.sentences = new string[] { "Получилось!",
                                                    "Теперь попробуй перепрыгнуть пропасть с помощью клавиши Space или кнопки A на геймпаде." };
                break;
            case 3:
                dialogue.name = "Обучение";
                dialogue.sentences = new string[] { "Замечательно.",
                                                    "Но не все так просто.",
                                                    "Ты же не думал, что игра ограничится бегом и прыжками?",
                                                    "Пройди немного вперед и ударь гоблина, используя клавишу F или кнопку X на геймпаде."};
                break;
            case 4:
                dialogue.name = "Обучение";
                dialogue.sentences = new string[] { "На самом деле, монстров можно убивать и прыжком.",
                                                    "После убийства врага прыжком, ты от него оттолкнешься.",
                                                    "Иногда это можно использовать в своих целях.",
                                                    "Попробуй достигнуть следующей платформы с помощью монстра."};
                break;
            case 5:
                dialogue.name = "Обучение";
                dialogue.sentences = new string[] { "Видимо, раз ты это читаешь, с заданием ты справился.",
                                                    "Самое время научить тебя подбирать монеты.",
                                                    "Просто подойди к монетке, чтобы добавить ее в свой мешочек с золотом.",
                                                    "Количество твоих монеток отображается в левом верхнем углу экрана."};
                break;
            case 6:
                dialogue.name = "Обучение";
                dialogue.sentences = new string[] { "Теперь попробуй подобрать баночку зелья.",
                                                    "С ее помощью сила твоего прыжка несколько увеличится на 10 секунд.",
                                                    "Попробуй допрыгнуть до следующей платформы, находясь под эффектом зелья."};
                break;
            case 7:
                dialogue.name = "Обучение";
                dialogue.sentences = new string[] { "Только что ты подобрал жизнь.",
                                                    "Жизни, как и монетки, отображаются в левом верхнем углу экрана.",
                                                    "Но не забывай, что жизней может быть только три, так что четвертую подобрать не получится.",
                                                    "Слышишь звук деревни? Направляйся туда, чтобы сменить локацию." };
                break;
        }
    }
    void WhatSignEng()

    {
        switch (NotifAnimator.GetInteger("SignNum"))
        {
            case 1:
                dialogue.name = "Instructions";
                dialogue.sentences = new string[] { "Welcome to the game!",
                                                    "You can use ← / → , A / D keys and left analog stick on your gamepad controller.",
                                                    "Let's try to reach next sign using this information."};
                break;
            case 2:
                dialogue.name = "Instructions";
                dialogue.sentences = new string[] { "You did it!",
                                                    "Now try jumping over the abyss using Space key or A button on the gamepad controller." };
                break;
            case 3:
                dialogue.name = "Instructions";
                dialogue.sentences = new string[] { "Excellent.",
                                                    "But not everything is so simple.",
                                                    "Did you think there is only pointless running and jumping in this game?",
                                                    "Go right ahead and hit an ugly goblin using F key or X button on your controller."};
                break;
            case 4:
                dialogue.name = "Instructions";
                dialogue.sentences = new string[] { "Actually, you can kill monsters by jumping on their heads.",
                                                    "After you kill someone by jumping on them, you'll bounce off that bastard.",
                                                    "Sometimes you can use it for your own good.",
                                                    "Try to reach the next platform using the monster ahead."};
                break;
            case 5:
                dialogue.name = "Instructions";
                dialogue.sentences = new string[] { "I guess if you're reading this, you did the job.",
                                                    "It's time to teach you how to collect coins.",
                                                    "Just walk up to it and it'll go right to your bag of gold coins.",
                                                    "The amount of your coins is displayed at the top left corner of the screen."};
                break;
            case 6:
                dialogue.name = "Instructions";
                dialogue.sentences = new string[] { "Now try picking up this potion.",
                                                    "Your jump force will slightly increase for 10 seconds.",
                                                    "Try reaching the next platform while affected by this potion."};
                break;
            case 7:
                dialogue.name = "Instructions";
                dialogue.sentences = new string[] { "You just picked up a life.",
                                                    "Lives are displayed at the top left corner of the screen, just as coins are.",
                                                    "But don't forget that lives' maximum is 3, so you can't pick up a fourth one.",
                                                    "Do you hear village sounds? Go check it out and change your location." };
                break;
        }
    }
}
