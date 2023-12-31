using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHelper : MonoBehaviour
{
    [SerializeField] TextAsset blocksJson;

    private List<Transform> blocks;
    private Platform platform;

    // Start is called before the first frame update
    void Start()
    {
        blocks = new List<Transform>();
        for (int i = 0; i < 4; i++)
        {
            // add corner
            Transform corner = transform.GetChild(0).GetChild(i);
            blocks.Add(corner);
            // add side
            Transform side = transform.Find($"Side{i+1}");
            for (int j = 0; j < side.childCount; j++)
            {
                blocks.Add(side.GetChild(j));
            }
        }

        platform = JsonUtility.FromJson<Platform>(blocksJson.text);

        for (int i = 0; i < blocks.Count; i++)
        {
            Transform blockTrans = blocks[i];
            Block blockInfo = platform.blocks[i];
            if (blockInfo.Color != null)
            {
                blockTrans.GetComponent<Renderer>().material.color = MyTools.ColorFromHex(blockInfo.Color);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetBlockTransform(int index)
    {
        return blocks[index];
    }

    public Transform GetWalkingPoint(int index)
    {
        return blocks[index].Find("WalkingPoint");
    }

    public Block GetBlock(int index)
    {
        return platform.blocks[index];
    }

    public Chance GetRandomChance()
    {
        int index = Random.Range(0, platform.chances.Length);
        return platform.chances[index];
    }

    public Destiny GetRandomDestiny()
    {
        int index = Random.Range(0, platform.destinies.Length);
        return platform.destinies[index];
    }
}
