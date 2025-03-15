using Godot;
using System;
using System.Xml.Linq;

public partial class CutSceneManager : Node
{
    private NPCPAMS _npcPams;
    private NPCDialogue _dialogue;
    private AudioStreamPlayer _player;
    private DialogPanel _panel;
    private int _currentCutScene = 0;
    private string _currentMusic;

    public CutSceneManager() =>
        Global.SceneObjects.OnDialoguePanelChanged += TakePanel;

    private void TakePanel(Node node)
    {
        _player = new AudioStreamPlayer();
        _panel = (DialogPanel)node;
        _panel.AddChild(_player);
    }

    public void OutputCutScene(int NPCID, int dialogueNumber)
    {
        if (_dialogue != null)
        {
            _dialogue = null;
            return;
        }
        Global.CutSceneData.GetCutSceneData(out _npcPams, out NPCDialogue npsDialogue, NPCID, dialogueNumber);
        StartCutScene(npsDialogue);
    }

    public void OutputCutScene(int NextDialogueNumber)
    {
        Global.CutSceneData.GetCutSceneData(out _npcPams, out NPCDialogue npsDialogue, _dialogue.NPCID, NextDialogueNumber, _dialogue.DialogueNumber);
        StartCutScene(npsDialogue);
    }

    public override void _Input(InputEvent @event)
    {
        if (_currentCutScene < _dialogue.Speech.Count - 1)
        {
            _currentCutScene++;
            _panel.NextDialogue(_currentCutScene);
            NextPAMS(_currentCutScene);
        }
        else
        {
            _currentCutScene = 0;
            _panel.EndDialogue();
        }
    }

    public void StartCutScene(NPCDialogue npcDialogue)
    {
        _panel.OutputSpeech(npcDialogue);
        _panel.NextDialogue(_currentCutScene);
        NextPAMS(_currentCutScene);
        _dialogue = npcDialogue;
        Global.Settings.CutScene = true;
    }

    public void NextPAMS(int currentCutScene)
    {
        if (_npcPams?.PAMSs[currentCutScene] != null)
        {
            if (_npcPams.PAMSs[currentCutScene].Music != null && _npcPams.PAMSs[currentCutScene].Music != _currentMusic)
            {
                Global.Sound.PlayMusic(_npcPams.PAMSs[currentCutScene].Music);
                _currentMusic = _npcPams.PAMSs[currentCutScene].Music;
                GD.Print(123);
            }
            if (_npcPams.PAMSs[currentCutScene].Sound != null)
                Global.Sound.PlaySound(_player, _npcPams.PAMSs[currentCutScene].Sound);
            if (_npcPams.PAMSs[currentCutScene].PAData != null)
                foreach (PAData paData in _npcPams.PAMSs[currentCutScene].PAData)
                    foreach (NPC npc in Global.SceneObjects.Npcs)
                        if (paData.NPCID == npc.ID)
                        {
                            if (paData?.StartPosition != null)
                                npc.Move((Vector2)paData.StartPosition, (Vector2)paData.EndPosition);
                            if (paData?.Animation != null)
                                npc.ChangeAnimation(paData.Animation);
                        }
        }
    }
}
