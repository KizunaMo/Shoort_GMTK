using System;
using Framework;
using ManagerDomain;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flavor : MonoBehaviour
{
    public Transform hairRoot;
    public Transform blinkRoot;
    public Transform eyesRoot;
    public Transform mouthRoot;
    public Transform makeupRoot;
    public Transform faceRoot;
    public Transform headRoot;
    public Transform decorationRoot;
    public Transform hipRoot;
    public Transform bodyRoot;
    public Transform footRoot;
    public Transform handRoot;

    public Transform hairStylesRoot;

    //GAME JAM WHAT EVER
    public Transform[] allHair;
    public Transform[] allBlink;
    public Transform[] allEyes;
    public Transform[] allMouth;
    public Transform[] allMakeup;
    public Transform[] allFace;
    public Transform[] allHead;
    public Transform[] allDecoration;
    public Transform[] allHip;
    public Transform[] allBody;
    public Transform[] allFoot;
    public Transform[] allHand;


    public Transform[] allHairStyles;


    public Transform[] CurrentHairStylePackage;

    public ResultFaceAggregate resultFaceAggregate;

    public void Init()
    {
        CheckAllRootChilds();

        RandomHairPackageStyle();

        RandomCustomerHairStyle();
        RandomBlink();
        RandomEyes();
        RandomMouth();
        RandomMakeup();
        RandomFace();
        RandomHead234();
        RandomDecoration();
        RandomHip();
        RandomBody();
        RandomFoot();
        RandomHand();

        UIManager.Instance.OnCut += CutAndChangeHairStyle;
        UIManager.Instance.OnEachTimerEnd += ShowResultFace;
    }

    private void ShowResultFace()
    {
        DisableRoot(mouthRoot);
        DisableRoot(eyesRoot);
        DisableRoot(blinkRoot);
        DisableRoot(makeupRoot);
        resultFaceAggregate.HideAll();
        //Amo.Instance.Log($"??? 成功還是失敗？ {isSuccessCutResult}");
        if (isSuccessCutResult)
        {
            
            resultFaceAggregate.ShowRandomHappiness();
        }
        else
        {
            resultFaceAggregate.ShowRandomSad();
        }
    }


    public void DisableRoot(Transform root)
    {
        root.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        UnregiserEvents();
    }

    public void UnregiserEvents()
    {
        UIManager.Instance.OnCut -= CutAndChangeHairStyle;
        UIManager.Instance.OnEachTimerEnd -= ShowResultFace;
    }


    public int GetHairCount()
    {
        var allHairCount = 3 + CurrentHairStylePackage.Length;
        // Amo.Instance.Log($"GetHairCount {allHairCount} // CurrentHairStylePackage count {CurrentHairStylePackage.Length}");
        return allHairCount;
    }


    //多砍第二刀
    public void DisplayHairRootAndDecorationRoot(bool isShow)
    {
        hairRoot.gameObject.SetActive(isShow);
        decorationRoot.gameObject.SetActive(isShow);
        HideAll(allHair);
        HideAll(allDecoration);
    }

    //多砍第三刀
    public void DisplayHeadOne(bool isShow)
    {
        var head1 = allHead[allHead.Length - 2];
        head1.gameObject.SetActive(isShow);
    }

    public void RandomHead234()
    {
        HideAll(allHead);
        var head = allHead[Random.Range(allHead.Length - 3, allHead.Length)];
        head.gameObject.SetActive(true);
    }

    //砍到不能再砍
    public void DisplayHead0(bool isShow)
    {
        allHead[allHead.Length - 1].gameObject.SetActive(isShow);
    }

    private bool HasAnyActiveChild(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.activeSelf)
                return true;
        }
        return false;
    }
    
    //private int currentHairIndex;

    public void RandomCustomerHairStyle()
    {
        var currentHairIndex = Random.Range(0, allHair.Length);
        HideAll(allHair);
        allHair[currentHairIndex].gameObject.SetActive(true);
    }

    private int currentHairPackageStyleIndex;

    public void RandomHairPackageStyle()
    {
        currentHairPackageStyleIndex = Random.Range(0, allHairStyles.Length);
        HideAll(allHairStyles);
        allHairStyles[currentHairPackageStyleIndex].gameObject.SetActive(true);

        Transform selectedStyle = allHairStyles[currentHairPackageStyleIndex];
        int childCount = selectedStyle.childCount;
        CurrentHairStylePackage = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            CurrentHairStylePackage[i] = selectedStyle.GetChild(i);
        }
    }

    private int currentBlinkIndex;

    public void RandomBlink()
    {
        currentBlinkIndex = Random.Range(0, allBlink.Length);
        HideAll(allBlink);
        allBlink[currentBlinkIndex].gameObject.SetActive(true);
    }

    private int currentEyesIndex;

    public void RandomEyes()
    {
        currentEyesIndex = Random.Range(0, allEyes.Length);
        HideAll(allEyes);
        allEyes[currentEyesIndex].gameObject.SetActive(true);
    }

    private int currentMouthIndex;

    public void RandomMouth()
    {
        currentMouthIndex = Random.Range(0, allMouth.Length);
        HideAll(allMouth);
        allMouth[currentMouthIndex].gameObject.SetActive(true);
    }

    private int currentMakeupIndex;

    public void RandomMakeup()
    {
        currentMouthIndex = Random.Range(0, allMakeup.Length);
        HideAll(allMakeup);
        allMakeup[currentMouthIndex].gameObject.SetActive(true);
    }

    private int currentFaceIndex;

    public void RandomFace()
    {
        currentFaceIndex = Random.Range(0, allFace.Length);
        HideAll(allFace);
        allFace[currentFaceIndex].gameObject.SetActive(true);
    }

    private int currentHeadIndex;

    public void RandomHead()
    {
        currentHeadIndex = Random.Range(0, allHead.Length - 1); //不要光頭
        ShowCurrentHead();
    }

    public void ShowCurrentHead()
    {
        HideAll(allHead);
        allHead[currentHeadIndex].gameObject.SetActive(true);
    }

    private int currentDecorationIndex;

    public void RandomDecoration()
    {
        currentDecorationIndex = Random.Range(0, allDecoration.Length);
        HideAll(allDecoration);
        allDecoration[currentDecorationIndex].gameObject.SetActive(true);
    }

    private int currentHipIndex;

    public void RandomHip()
    {
        currentHipIndex = Random.Range(0, allHip.Length);
        HideAll(allHip);
        allHip[currentHipIndex].gameObject.SetActive(true);
    }

    private int currentBodyIndex;

    public void RandomBody()
    {
        currentBodyIndex = Random.Range(0, allBody.Length);
        HideAll(allBody);
        allBody[currentBodyIndex].gameObject.SetActive(true);
    }

    private int currentFootIndex;

    public void RandomFoot()
    {
        currentFootIndex = Random.Range(0, allFoot.Length);
        HideAll(allFoot);
        allFoot[currentFootIndex].gameObject.SetActive(true);
    }

    private int currentHandIndex;

    public void RandomHand()
    {
        currentHandIndex = Random.Range(0, allHand.Length);
        HideAll(allHand);
        allHand[currentHandIndex].gameObject.SetActive(true);
    }

    public void HideAll(Transform[] allTrans)
    {
        for (int i = 0; i < allTrans.Length; i++)
        {
            allTrans[i].gameObject.SetActive(false);
        }
    }


    public void DisplayAllCurrentHairStylePackage(bool isShow)
    {
        for (int i = 0; i < CurrentHairStylePackage.Length; i++)
        {
            CurrentHairStylePackage[i].gameObject.SetActive(isShow);
        }
    }


    private bool cutedNoMore;
    private bool cuted2;
    private bool cuted3;

    private bool isSuccessCutResult;
    private int GetActiveChildrenCount(Transform parent)
    {
        int count = 0;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.activeSelf)
                count++;
        }
        return count;
    }
    private void CutAndChangeHairStyle()
    {
        var root = allHairStyles[currentHairPackageStyleIndex];
        var hasAnyActiveChild = HasAnyActiveChild(root);
        var childTuple = FindNextOfFirstActiveChild(root);
        
        // Amo.Instance.Log($"=== CutAndChangeHairStyle Debug ===");
        // Amo.Instance.Log($"allInactive: {hasAnyActiveChild}");
        // Amo.Instance.Log($"cuted2: {cuted2}, cuted3: {cuted3}, cutedNoMore: {cutedNoMore}");
        // Amo.Instance.Log($"Active children count: {GetActiveChildrenCount(root)}");
        
        if (hasAnyActiveChild)
        {
            isSuccessCutResult = false;
            // Amo.Instance.Log($"A {gameObject.name}");
            if (childTuple.nextItem != null)
            {
                //Amo.Instance.Log($"Check first active child => {childTuple.activeItem.gameObject.name} // {childTuple.nextItem.gameObject.name}",Color.cyan);
                childTuple.nextItem.gameObject.SetActive(true);
                childTuple.activeItem.gameObject.SetActive(false);
                if (childTuple.nextItem.gameObject.name == Consts.CustomKeywords.SuccessCutHairIndex)
                {
                    //Amo.Instance.Log($"A 成功 Checking hair {childTuple.activeItem.name}");
                    isSuccessCutResult = true;
                }
           }        
            //Amo.Instance.Log($"Checking hair {childTuple.activeItem.name} // {childTuple.nextItem.name}");
            if (childTuple.activeItem != null && childTuple.activeItem.gameObject.name == Consts.CustomKeywords.SuccessCutHairIndex)
            {
                //Amo.Instance.Log($"BChecking hair {childTuple.activeItem.name}");
                childTuple.activeItem.gameObject.SetActive(false);
                isSuccessCutResult = false;
            }
            
            //Amo.Instance.Log($"???? isSuccessCutResult {isSuccessCutResult}",Color.green);
        }
        else
        {
            isSuccessCutResult = false;
            //Amo.Instance.Log($"B {gameObject.name}");
            if (!cuted2)
            {
                // Amo.Instance.Log($"多砍第二刀");
                DisplayHairRootAndDecorationRoot(false);
                cuted2 = true;
                return;
            }

            if (!cuted3)
            {
                // Amo.Instance.Log($"多砍第３刀");
                headRoot.gameObject.SetActive(true);
                ShowCurrentHead();
                DisplayHeadOne(true);
                cuted3 = true;
                return;
            }

            if (!cutedNoMore)
            {
                // Amo.Instance.Log($"砍到不能再砍");
                headRoot.gameObject.SetActive(true);
                HideAll(allHead);
                DisplayHead0(true);
                cutedNoMore = true;
                return;
            }
        }
    }

    (Transform activeItem, Transform nextItem) FindNextOfFirstActiveChild(Transform root)
    {
        // Amo.Instance.Log("FindNextOfFirstActiveChild");

        for (int i = 0; i < root.childCount; i++)
        {
            var child = root.GetChild(i);
            // Amo.Instance.Log($"Check {child.name} active => {child.gameObject.activeInHierarchy}");

            if (child.gameObject.activeInHierarchy)
            {
                int nextIndex = i + 1;

                if (nextIndex < root.childCount)
                {
                    var nextChild = root.GetChild(nextIndex);
                    // Amo.Instance.Log($"Found active at {child.name}, next is {nextChild.name}");
                    return (child, nextChild);
                }
                else
                {
                    // Amo.Instance.Log($"Found active at {child.name}, but it's the last one => return null");
                    return (child, null);
                }
            }
        }

        // Amo.Instance.Log("No active child found => return null");
        return (null, null);
    }

    public static bool IsOnlyLastChildActive(Transform parentTransform, bool checkHierarchy = false)
    {
        if (parentTransform == null)
            throw new ArgumentNullException(nameof(parentTransform));
    
        int childCount = parentTransform.childCount;
        if (childCount == 0)
            return false;
    
        int lastChildIndex = childCount - 1;
        bool lastChildIsActive = false;
    
        for (int i = 0; i < childCount; i++)
        {
            bool isChildActive = checkHierarchy 
                ? parentTransform.GetChild(i).gameObject.activeInHierarchy
                : parentTransform.GetChild(i).gameObject.activeSelf;
        
            if (i == lastChildIndex)
            {
                lastChildIsActive = isChildActive;
            }
            else if (isChildActive)
            {
                return false;
            }
        }
    
        return lastChildIsActive;
    }


    private void CheckAllRootChilds()
    {
        if (hairRoot != null)
        {
            allHair = new Transform[hairRoot.childCount];
            for (int i = 0; i < hairRoot.childCount; i++)
            {
                allHair[i] = hairRoot.GetChild(i);
            }
        }

        if (blinkRoot != null)
        {
            allBlink = new Transform[blinkRoot.childCount];
            for (int i = 0; i < blinkRoot.childCount; i++)
            {
                allBlink[i] = blinkRoot.GetChild(i);
            }
        }

        if (eyesRoot != null)
        {
            allEyes = new Transform[eyesRoot.childCount];
            for (int i = 0; i < eyesRoot.childCount; i++)
            {
                allEyes[i] = eyesRoot.GetChild(i);
            }
        }

        if (mouthRoot != null)
        {
            allMouth = new Transform[mouthRoot.childCount];
            for (int i = 0; i < mouthRoot.childCount; i++)
            {
                allMouth[i] = mouthRoot.GetChild(i);
            }
        }

        if (makeupRoot != null)
        {
            allMakeup = new Transform[makeupRoot.childCount];
            for (int i = 0; i < makeupRoot.childCount; i++)
            {
                allMakeup[i] = makeupRoot.GetChild(i);
            }
        }

        if (faceRoot != null)
        {
            allFace = new Transform[faceRoot.childCount];
            for (int i = 0; i < faceRoot.childCount; i++)
            {
                allFace[i] = faceRoot.GetChild(i);
            }
        }

        if (headRoot != null)
        {
            allHead = new Transform[headRoot.childCount];
            for (int i = 0; i < headRoot.childCount; i++)
            {
                allHead[i] = headRoot.GetChild(i);
            }
        }

        if (decorationRoot != null)
        {
            allDecoration = new Transform[decorationRoot.childCount];
            for (int i = 0; i < decorationRoot.childCount; i++)
            {
                allDecoration[i] = decorationRoot.GetChild(i);
            }
        }

        if (hipRoot != null)
        {
            allHip = new Transform[hipRoot.childCount];
            for (int i = 0; i < hipRoot.childCount; i++)
            {
                allHip[i] = hipRoot.GetChild(i);
            }
        }

        if (bodyRoot != null)
        {
            allBody = new Transform[bodyRoot.childCount];
            for (int i = 0; i < bodyRoot.childCount; i++)
            {
                allBody[i] = bodyRoot.GetChild(i);
            }
        }

        if (footRoot != null)
        {
            allFoot = new Transform[footRoot.childCount];
            for (int i = 0; i < footRoot.childCount; i++)
            {
                allFoot[i] = footRoot.GetChild(i);
            }
        }

        if (handRoot != null)
        {
            allHand = new Transform[handRoot.childCount];
            for (int i = 0; i < handRoot.childCount; i++)
            {
                allHand[i] = handRoot.GetChild(i);
            }
        }

        if (hairStylesRoot != null)
        {
            allHairStyles = new Transform[hairStylesRoot.childCount];
            for (int i = 0; i < hairStylesRoot.childCount; i++)
            {
                allHairStyles[i] = hairStylesRoot.GetChild(i);
            }
        }
    }
}