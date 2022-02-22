using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class NPCCRandomizer : MonoBehaviour
    {
        [Header("Refs")]
        public CrewmateNPC npcData;
        public FollowSimple viewDetection;
        public FollowSimple areaListener;

        [Header("General Traits")]
        public Color[] hairColors;
        public Vector2 skinColorHueRange;
        public Vector2 skinColorDarkRange;

        [Header("Male")]
        public GameObject maleBase;
        public GameObject maleHeadBone;
        public SkinnedMeshRenderer maleSkin;
        public List<GameObject> maleHairstyles;

        [Header("Female")]
        public GameObject femaleBase;
        public GameObject femaleHeadBone;
        public SkinnedMeshRenderer femaleSkin;
        public List<GameObject> femaleHairstyles;

        GameObject body;
        GameObject hair;
        Color hairColor;
        bool isMale;

        int shaderID_Hue;
        int shaderID_Darkness;

        private void Awake()
        {
            shaderID_Hue = Shader.PropertyToID("_Hue");
            shaderID_Hue = Shader.PropertyToID("_Darkness");
        }
        void Start()
        {
            RandomizeTraits();
        }

        // Update is called once per frame
        void Update()
        {

        }

        [Button("Randomize Traits")]
        public void RandomizeTraits()
        {
            isMale = Random.Range(0, 2) > 0;
            maleBase.SetActive(isMale);
            femaleBase.SetActive(!isMale);
            body = isMale ? maleBase : femaleBase;

            var skinHueValue = Random.Range(skinColorHueRange.x, skinColorHueRange.y);
            var skinDarkValue = Random.Range(skinColorDarkRange.x, skinColorDarkRange.y);

            if (isMale)
            {
                hair = maleHairstyles[Random.Range(0, maleHairstyles.Count)];
                maleHairstyles.ForEach(h => h.SetActive(h == hair));
                maleSkin.material.SetFloat(shaderID_Hue, skinHueValue);
                maleSkin.material.SetFloat(shaderID_Darkness, skinDarkValue);
                npcData.animator = maleBase.GetComponent<Animator>();
                npcData.agent = maleBase.GetComponent<NavMeshAgent>();
                areaListener.followTarget = maleBase.transform;
                viewDetection.followTarget = maleHeadBone.transform;
            }
            else
            {
                hair = maleHairstyles[Random.Range(0, femaleHairstyles.Count)];
                femaleHairstyles.ForEach(h => h.SetActive(h == hair));
                femaleSkin.material.SetFloat(shaderID_Hue, skinHueValue);
                femaleSkin.material.SetFloat(shaderID_Darkness, skinDarkValue);
                npcData.agent = femaleBase.GetComponent<NavMeshAgent>();
                npcData.animator = femaleBase.GetComponent<Animator>();
                areaListener.followTarget = femaleBase.transform;
                viewDetection.followTarget = femaleHeadBone.transform;
            }

            hairColor = hairColors[Random.Range(0, hairColors.Length)];
            hair.GetComponent<MeshRenderer>().material.color = hairColor;
        }
    }
}
