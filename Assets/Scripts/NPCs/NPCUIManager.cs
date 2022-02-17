using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPCUIManager : MonoBehaviour
    {
        public GenericNPC npcData;

        [Header("Suspicion")]
        //public Animator qmTransforms;
        public SpriteRenderer qmSprite;
        public float qmAnimTime = 1;
        public Ease qmInEase;
        public Ease qmOutEase;

        public UEventHandler eventHandler = new UEventHandler();

        Material questionMarkMat;
        int susShaderParamId;
        bool isSusShown;
        Vector3 qmInitScale;

        private void Awake()
        {
            qmInitScale = qmSprite.transform.localScale;
            qmSprite.transform.localScale = Vector3.zero;
            susShaderParamId = Shader.PropertyToID("_Value");

            npcData.OnBeginSus.Subscribe(eventHandler, () =>  SusChanged(true));
            npcData.OnEndSus.Subscribe(eventHandler, () => SusChanged(false));
        }

        private void OnDestroy()
        {
            eventHandler.UnsubcribeAll();
        }
        void Start()
        {
            questionMarkMat = qmSprite.material;
        }

        // Update is called once per frame
        void Update()
        {
            if (isSusShown)
            {
                questionMarkMat.SetFloat(susShaderParamId, npcData.susLevel);
            }
        }

        private void SusChanged(bool showSus)
        {
            isSusShown = showSus;

            if (isSusShown)
                qmSprite.transform.DOScale(qmInitScale, qmAnimTime).SetEase(qmInEase);
            //qmAnimator.SetTrigger("Show");
            else
                qmSprite.transform.DOScale(Vector3.zero, qmAnimTime).SetEase(qmOutEase);
            //qmAnimator.SetTrigger("Unshow");
        }
    }
}
