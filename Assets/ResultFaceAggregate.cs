using UnityEngine;

public class ResultFaceAggregate : MonoBehaviour
{
   public Transform[] happy;
   public Transform[] sad;

   public void ShowRandomHappiness()
   {
      var random = Random.Range(0, happy.Length-1);
      happy[random].gameObject.SetActive(true);
   }

   public void ShowRandomSad()
   {
      var random = Random.Range(0, sad.Length-1);
      sad[random].gameObject.SetActive(true);
   }
   public void HideAll()
   {
      for (int i = 0; i < happy.Length; i++)
      {
         happy[i].gameObject.SetActive(false);
      }
      for (int i = 0; i < sad.Length; i++)
      {
         sad[i].gameObject.SetActive(false);
      }
   }
}
