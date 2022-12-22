using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameCommand : ICommand
{
    LevelLoader levelLoader;

    public EndGameCommand(LevelLoader levelLoader)
    {
        this.levelLoader = levelLoader;
    }

    public void Execute()
    {
        PanelHolder.holder.endGamePannel.SetActive(true);
    }
}
