using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RandomWalker
{
	public enum DIRECTION
	{
		UP, 
		DOWN, 
		LEFT, 
		RIGHT,
		
		MAX_COUNT
	}

	public Vector2 m_currPos;
	private DIRECTION m_direction;
	
	DIRECTION SelectDirection()
	{
		return (DIRECTION)Random.Range(0, (int)DIRECTION.MAX_COUNT);
	}
	public void UpdateWalker()
	{
		m_direction = SelectDirection();
		switch(m_direction) 
		{
			case DIRECTION.UP:
				m_currPos.y += 1;
				break;
			case DIRECTION.DOWN:
				m_currPos.y -= 1;
				break;
			case DIRECTION.LEFT:
				m_currPos.x -= 1;
				break;	
			case DIRECTION.RIGHT:
				m_currPos.x += 1;
				break;
		}
	}
	public DIRECTION GetWalkerDirection()
	{
		return m_direction;
	}
	public void SetWalkerPosition(Vector2 pos)
	{
		m_currPos = pos;
	}
	public bool CheckOutOfBoundGrid(int x, int y)
	{
		//check out of boundary
		int currPosX = (int)m_currPos.x;
		int currPosY = (int)m_currPos.y;
		int halfExtentX = (int)(x * 0.5f);
		int halfExtentY = (int)(y * 0.5f);
		return (currPosX > halfExtentX || currPosX < -halfExtentX - 1 || currPosY > halfExtentY || currPosY < -halfExtentY -1);
	}
	public bool CheckOutOfBoundGrid(Room[,] rooms)
	{
		return CheckOutOfBoundGrid(rooms.GetLength(0),rooms.GetLength(1));
			
	}
	public Vector2 GetCurrPosition()
	{
		return m_currPos;
	}
	public RandomWalker(Vector2 currPos)
	{
		m_currPos = currPos;
	}
}
