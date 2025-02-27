namespace DG.Tweening
{
    public class DOTweenAddition
    {
        public static void Play(string animID)
        {
            if (DOTween.IsTweening(animID)) DOTween.Kill(animID);
            else DOTween.Play(animID);
        }
    }
}
