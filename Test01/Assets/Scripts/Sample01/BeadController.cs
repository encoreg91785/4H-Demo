using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeadController : MonoBehaviour
{
    static BeadController _instance;
    public static BeadController instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<BeadController>();
            return _instance;
        }
    }
    [SerializeField]
    BeadItem beadItem;
    [SerializeField]
    GameObject allPonitsParent,beadParent;
    Transform[] allPonits;
    List<BeadItem> AllBead = new List< BeadItem>();
    Color[] colors = new Color[] {Color.black,Color.blue, Color.yellow,Color.red,Color.green};
    // Use this for initialization
    void Start ()
    {
        allPonits = allPonitsParent.GetComponentsInChildren<Transform>();
        InitBead();
    }

    public void OnBeginDrag(BeadItem bead)
    {
        bead.transform.SetAsFirstSibling();
    }

    public void OnDrag(BeadItem bead)
    {
        var rect = beadParent.GetComponent<RectTransform>();
        var x = Mathf.Clamp(Input.mousePosition.x, rect.position.x - rect.sizeDelta.x/2 + 15, rect.position.x + rect.sizeDelta.x/2-15);
        var y = Mathf.Clamp(Input.mousePosition.y, rect.position.y - rect.sizeDelta.y/2 + 15, rect.position.y + rect.sizeDelta.y/2 - 15);
        bead.transform.position =new Vector2(x, y);
        foreach (var item in AllBead)
        {
            if (item.bId == bead.bId) continue;
            var dis = Vector2.Distance(item.transform.position, bead.transform.position);
            if (dis < 40f && item.trigger == false)
            {
                ExChangePoint(bead, item);
            }
            else if (dis > 40f)
            {
                item.trigger = false;
            }
            if (Vector2.Distance(Input.mousePosition, allPonits[bead.bId].transform.position) > 65f)
            {
                var disMin = 100000f;
                int index = 0;
                for (int i = 1; i < allPonits.Length; i++)
                {
                    if (Vector2.Distance(allPonits[i].position, bead.transform.position) < disMin)
                    {
                        disMin = Vector2.Distance(allPonits[i].position, bead.transform.position);
                        index = i;
                    }
                }
               if(index!= bead.bId) ExChangePoint(bead, AllBead.Find((b) => { return b.bId == index; }));
            }
        }
    }

    public void OnEndDrag(BeadItem bead)
    {
        InvokeRepeating("CheckLink", 0.5f,1);
    }

    public void OnClickUp(BeadItem bead)
    {
        bead.transform.position = allPonits[bead.bId].position;
    }

    bool CheckLink()
    {
        Dictionary<Color, List<BeadItem>> dic = new Dictionary<Color, List<BeadItem>>();
        List<int> lineList = new List<int>();
        for (int i = 1; i < allPonits.Length; i++)
        {
            var currentBead = AllBead.Find((b) => { return b.bId == i; });
            List<BeadItem> bList = new List<BeadItem>();
            for (int j = 0; FindNextBeadIsSameColorRaw(i + j, currentBead.color)!=null && j < 5; j++)
            {
                if (j == 1)
                {
                    bList.Add(currentBead);
                    for (int k = 0; k < 2; k++)
                    {
                        var _bead = FindNextBeadIsSameColorRaw(i + k, currentBead.color);
                        bList.Add(_bead);
                    }
                }
                else if (j > 2)
                {
                    var _bead = FindNextBeadIsSameColorRaw(i + j, currentBead.color);
                    bList.Add(_bead);
                }
            }
            for (int j = 0; FindNextBeadIsSameColorLine(i + j, currentBead.color)!=null && j < 20; j+=5)
            {
                if (j == 5)
                {
                    bList.Add(currentBead);
                    for (int k = 0; k < 6; k+=5)
                    {
                        var _bead = FindNextBeadIsSameColorLine(i + k, currentBead.color);
                        bList.Add(_bead);
                    }
                }
                else if (j > 6)
                {
                    var _bead = FindNextBeadIsSameColorLine(i + j, currentBead.color);
                    bList.Add(_bead);
                }
            }
            if (dic.ContainsKey(currentBead.color) == false) dic.Add(currentBead.color, bList);
            else dic[currentBead.color].AddRange(bList);
        }
        bool forf = false;
        foreach (var dd in dic)
        {
            foreach (var item in dd.Value)
            {
                AllBead.Remove(item);
                Destroy(item.gameObject);
                forf = true;
            }
        }
        FallingBead();
        if (forf == false) CancelInvoke("CheckLink");
        return forf;
    }

    void FallingBead()
    {
        List<int> emptyPoint = new List<int>();
        for (int i = 1; i <= 25; i++)
        {
            if(AllBead.Find((b)=> { return b.bId == i; })==null) emptyPoint.Add(i);
        }
        AllBead.Sort((aa,bb)=> { return aa.bId > bb.bId ? -1 : 1; });
        for (int i = 0; i < AllBead.Count; i++)
        {
            if (AllBead[i].bId > 20) continue;
            for (int j = AllBead[i].bId+20; j>0&&j> AllBead[i].bId; j-=5)
            {
                if (emptyPoint.Contains(j)==true)
                {
                    emptyPoint.Remove(j);
                    emptyPoint.Add(AllBead[i].bId);
                    AllBead[i].bId = j;
                    AllBead[i].MoveTo(allPonits[j].position);
                    break;
                }
            }
        }
        for (int i = 0; i < emptyPoint.Count; i++)
        {
            var go = CreateBead();
            go.bId = emptyPoint[i];
            go.transform.position = allPonits[emptyPoint[i]].position + allPonits[emptyPoint[i]].up * 500;
            go.MoveTo(allPonits[emptyPoint[i]].position);
        }
        
    }

    /// <summary>
    /// 左到右
    /// </summary>
    /// <param name="id"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    BeadItem FindNextBeadIsSameColorRaw(int id,Color color)
    {
        if (id % 5 == 0) return null;
        var bead = AllBead.Find((b) => { return b.bId == id + 1; });
        if (bead.color == color)return bead;
        else return null;
    }

    /// <summary>
    /// 上到下
    /// </summary>
    /// <param name="id"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    BeadItem FindNextBeadIsSameColorLine(int id, Color color)
    {
        if ((id-1) / 5 > 3) return null;
        var bead = AllBead.Find((b) => { return b.bId == id + 5; });
        if (bead.color == color)return bead;
        else return null;
    }

    void ExChangePoint(BeadItem selectBead, BeadItem tagetbead)
    {
        var id = selectBead.bId;
        selectBead.bId = tagetbead.bId;
        tagetbead.bId = id;
        tagetbead.trigger = true;
        tagetbead.MoveTo(allPonits[id].position);
    }

    void InitBead()
    {
        for (int i = 1; i < allPonits.Length; i++)
        {
            var go = CreateBead();
            go.bId = i;
            go.transform.position = allPonits[i].transform.position;
        }
    }

    BeadItem CreateBead()
    {
        var obj = Instantiate(beadItem.gameObject, beadParent.transform);
        obj.SetActive(true);
        var go = obj.GetComponent<BeadItem>();
        go.color = colors[Random.Range(0, colors.Length)];
        AllBead.Add(go);
        return go;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
