using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class SetVolumeAsPriority : MonoBehaviour
{
    
    [SerializeField] List<MasterAudioGroup> masterAudioGroups;
    int roomSize;
    int availableAudioCount;
    List<string> room = new List<string>();

    private void Awake()
    {
        roomSize = masterAudioGroups.Count;
    }

    public void AddAudio(bool isPlay, string audioName)
    {
        if (isPlay)
        {
            room.Add(audioName);
            availableAudioCount++;
        }
        else
        {
            room.Add(null);
        }
        
    }

    private void Update()
    {
        if (room.Count == roomSize)
        {
            foreach (var audioGroup in masterAudioGroups)
            {
                string audioName = audioGroup.gameObject.name;
                if (room.Contains(audioName) && (audioGroup.importance != 4 || availableAudioCount == 1 ))//만약 importance가 4라면 실행될 오디오가 자기 자신 뿐일 때 실행
                {
                    MasterAudio.PlaySound(audioName);
                }
                
            }
            room.Clear();
            availableAudioCount = 0;
        }
    }
}
