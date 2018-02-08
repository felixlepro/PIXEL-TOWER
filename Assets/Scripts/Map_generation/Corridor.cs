using UnityEngine;

public enum Direction
    {
        Nord,Est,Sud,Ouest,
    }
public class Corridor
{
    public int startX;
    public int startY;
    public int longCorridor;
    public Direction direction;

    public int EndPosX
    {
        get
        {
            if (direction == Direction.Nord || direction == Direction.Sud)
                return startX;
            if (direction == Direction.Est)
                return startX + longCorridor - 1;
            else
                return startX - longCorridor + 1;
      
        }
    }
    public int EndPosY
    {
        get
        {
            if (direction == Direction.Est  || direction == Direction.Ouest )
                return startY;
            if (direction == Direction.Nord )
                return startY + longCorridor - 1;
            else
                return startY - longCorridor + 1;

        }
    }
    public void SetupCorridor(Room room, IntRange longueur,IntRange largRoom, IntRange hautRoom, int columns, int rows,bool corridor1)
    {
        direction = (Direction)Random.Range(0, 4);
        Direction directionCont = (Direction)(((int)room.enteringCorridor + 2) % 4);
        if (!corridor1 && direction == directionCont )
             direction= (Direction)(((int)direction + 1) % 4);

        longCorridor = longueur.Random;
        int longMax = longueur.max;
        switch (direction)
        {
                case Direction.Nord:
                startX = Random.Range(room.posX, room.posX + room.largRoom  - 1);
                startY = room.posY + room.hautRoom;
                longMax = rows - startY - hautRoom.min;
                break;
            case Direction.Est:
                startX = room.posX + room.largRoom;
                startY = Random.Range(room.posY, room.posY + room.hautRoom - 1);
                longMax = columns - startX - largRoom.min;
                break;
            case Direction.Sud:
                startX = Random.Range(room.posX, room.posX + room.largRoom);
                startY = room.posY;
                longMax = startY - hautRoom.min;
                break;
            case Direction.Ouest:
                startX = room.posX;
                startY = Random.Range(room.posY, room.posY + room.hautRoom);
                longMax = startX - largRoom.min;
                break;
        }

        
        longCorridor  = Mathf.Clamp(longCorridor , 1, longMax);
    }

}
