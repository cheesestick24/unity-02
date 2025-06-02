using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

public class BlockGenerator : MonoBehaviour
{
    public GameObject blockPrefab;

    // ブロックの横の数
    private int blocksPerRowDefault = 15;
    private int minBlocksPerRow = 5;
    private int maxBlocksPerRow = 15;

    // ブロックの間隔
    private float blockSpacingX = 1.1f;
    // ブロックの生成開始位置
    private float startX = -7.8f;
    private float startY = 0f;
    private float startZ = 4f;

    // ブロックの落下速度
    public float blockSlideSpeed = 0.1f;
    // 現在のブロックのリスト
    private List<GameObject> currentBlocks = new();

    public void GenerateNewBlockRow()
    {
        GenerateNewBlockRow(false);
    }

    /// <summary>
    /// 新しいブロックの行を生成します。
    /// </summary>
    /// <param name="isRandom">ランダムなブロック数を生成するかどうか</param>
    public void GenerateNewBlockRow(bool isRandom)
    {
        if (blockPrefab == null)
        {
            Debug.LogError("ブロックのプレハブが設定されていません！BlockGeneratorのインスペクターで設定してください。");
            return;
        }

        int actualBlocksPerRow;
        if (isRandom)
        {
            // ランダムなブロック数を決定
            actualBlocksPerRow = Random.Range(minBlocksPerRow, maxBlocksPerRow + 1); // +1 は Random.Range の上限がexclusiveのため
        }
        else
        {
            // デフォルトのブロック数を設定
            actualBlocksPerRow = blocksPerRowDefault;
        }

        float totalRowWidth = (actualBlocksPerRow - 1) * blockSpacingX;
        float adjustedStartX = startX - (totalRowWidth / 2f) + (blocksPerRowDefault - 1) * blockSpacingX / 2f;

        for (int i = 0; i < actualBlocksPerRow; i++)
        {
            Vector3 spawnPosition = new Vector3(
                adjustedStartX + (i * blockSpacingX),
                startY,
                startZ
            );
            GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
            Renderer blockRenderer = newBlock.GetComponent<Renderer>();
            if (blockRenderer != null)
            {
                blockRenderer.material.color = new Color(Random.value, Random.value, Random.value, 1f);
            }
            currentBlocks.Add(newBlock);
        }
        Debug.Log("新しいブロックの行が生成されました！");
    }

    /// <summary>
    /// 既存のブロックを前方に移動させます。
    /// </summary>
    public void MoveExistingBlocksForward()
    {
        for (int i = currentBlocks.Count - 1; i >= 0; i--)
        {
            GameObject block = currentBlocks[i];
            if (block != null)
            {
                block.transform.position += Vector3.forward * -blockSlideSpeed * Time.deltaTime;
            }
            else
            {
                currentBlocks.RemoveAt(i);
            }
        }
    }
}
