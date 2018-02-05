using UnityEngine;

public class Room
{
    public int posX;
    public int posY;
    public int largRoom;
    public int hautRoom;
    public Direction enteringCorridor;
    
    public void SetupRoom(IntRange rangeLarg, IntRange  rangeHaut, int columns,int rows)
    {
        largRoom = rangeLarg.Random;
        hautRoom = rangeHaut.Random;

        posX = Mathf.RoundToInt(columns / 2f - largRoom / 2f);
        posY = Mathf.RoundToInt(rows / 2f - hautRoom / 2f);
        
    }
	public void SetupRoom(IntRange rangeLarg, IntRange rangeHaut, int columns, int rows, Corridor corridor)
    {
        enteringCorridor = corridor.direction;
        largRoom = rangeLarg.Random;
        hautRoom = rangeHaut.Random;
        switch (corridor.direction)
        {
           case Direction.Nord:
                hautRoom = Mathf.Clamp(hautRoom, 1, rows - corridor.EndPosY);
                posY = corridor.EndPosY;
                posX = Random.Range(corridor.EndPosX - largRoom + 1, corridor.EndPosX);

                posX = Mathf.Clamp(posX, 0, columns - largRoom);
                break;
            case Direction.Est:
                largRoom = Mathf.Clamp(largRoom, 1, columns - corridor.EndPosX);
                posX = corridor.EndPosX;

                posY = Random.Range(corridor.EndPosY - hautRoom + 1, corridor.EndPosY);
                posY = Mathf.Clamp(posY, 0, rows - hautRoom);
                break;
            case Direction.Sud:
                hautRoom = Mathf.Clamp(hautRoom, 1, corridor.EndPosY);
                posY = corridor.EndPosY - hautRoom + 1;

                posX = Random.Range(corridor.EndPosX - largRoom + 1, corridor.EndPosX);
                posX = Mathf.Clamp(posX, 0, columns - largRoom);
                break;
            case Direction.Ouest:
                largRoom = Mathf.Clamp(largRoom, 1, corridor.EndPosX);
                posX = corridor.EndPosX - largRoom + 1;

                posY = Random.Range(corridor.EndPosY - hautRoom + 1, corridor.EndPosY);
                posY = Mathf.Clamp(posY, 0, rows - hautRoom);
                break;
        }
    }
}
