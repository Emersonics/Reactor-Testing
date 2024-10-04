/* This file was auto-generated. DO NOT MODIFY THIS FILE. */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using KS.Reactor.Client.Unity;
using KS.Reactor;
using KS.Unity;

namespace KSProxies.Scripts
{
    [CreateAssetMenu(menuName = ksMenuNames.REACTOR + "AvatarController", order = ksMenuGroups.SCRIPT_ASSETS)]
    public class AvatarController : ksPlayerControllerAsset
    {

        public Single m_speed;
        public Single m_turnSpeed;
        public Single m_jumpSpeed;
        public Single m_shootInterval;
        public AvatarController() : base() 
        {
            m_speed = 5f;
            m_turnSpeed = 360f;
            m_jumpSpeed = 6f;
            m_shootInterval = 0.3333333f;
            m_useInputPrediction = true;
        }

    }
}