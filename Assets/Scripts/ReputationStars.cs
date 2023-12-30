using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationStars : MonoBehaviour
{
	public static ReputationStars i;

    [SerializeField] private Image[] stars;

	[SerializeField] private Sprite wholeStar;
	[SerializeField] private Sprite halfStar;

	void Start()
	{
		i = this;
		UpdateStars();
	}

	public void UpdateStars()
	{
		int wholeStars = Mathf.FloorToInt(Game.i.Reputation / 2f);
		int halfStars = Mathf.FloorToInt(Game.i.Reputation) - wholeStars * 2;

		for (int i = 0; i < 5; i++)
		{
			stars[i].enabled = false;
		}

		for (int i = 0; i < wholeStars; i++)
		{
			stars[i].enabled = true;
			stars[i].sprite = wholeStar;
		}

		if (halfStars == 1)
		{
			stars[wholeStars].enabled = true;
			stars[wholeStars].sprite = halfStar;
		}
	}
}
