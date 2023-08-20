using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using OWOHaptic;
using OwoFunctional;
using System.Resources;
using System.Globalization;
using System.Collections;


namespace MyOWOVest
{
    public class TactsuitVR
    {
        /* A class that contains the basic functions for the bhaptics Tactsuit, like:
         * - A Heartbeat function that can be turned on/off
         * - A function to read in and register all .tact patterns in the bHaptics subfolder
         * - A logging hook to output to the Melonloader log
         * - 
         * */
        public bool suitDisabled = true;
        public bool systemInitialized = false;
        // Event to start and stop the heartbeat thread
        public Dictionary<String, ISensation> FeedbackMap = new Dictionary<String, ISensation>();


        /*
        //public static ISensation Explosion => new Sensation(100, 1f, 80, 100f, 500f, 0f);
        public static Sensation Explosion = Sensation.Create(100, 1f, 80, 100f, 500f, 0f);
        public static ISensation ExplosionBelly = Sensation.CreateWithMuscles(Explosion, Muscle.Lumbar_L, Muscle.Lumbar_R, Muscle.Abdominal_L, Muscle.Abdominal_R);
        //public static OWOSensationWithMuscles ExplosionBelly = new OWOSensationWithMuscles(Explosion, OWOMuscle.Abdominal_Left, OWOMuscle.Abdominal_Right, OWOMuscle.Lumbar_Left, OWOMuscle.Lumbar_Right);

        public static Sensation Healing = Sensation.Create(70, 0.5f, 65, 300f, 200f, 0f);
        public static ISensation HealingBody = Sensation.CreateWithMuscles(Healing, Muscle.AllMuscles);

        
        public static Sensation Reload1 = Sensation.Create(100, 0.3f, 50, 100f, 100f, 0f);
        public static Sensation Reload2 = Sensation.Create(100, 0.2f, 40, 0f, 100f, 0f);
        public static ISensation Reloading = Reload1.ContinueWith(Reload2);
        */

        public TactsuitVR()
        {
            InitializeOWO();
        }

        private async void InitializeOWO()
        {
            LOG("Initializing suit");

            string IPFile = Directory.GetCurrentDirectory() + "\\Mods\\OWO\\IP.txt";
            if(File.Exists(IPFile))
            {
                string IP=File.ReadAllText(IPFile);
                await OWO.Connect(IP);
            }
            else
            {
                await OWO.AutoConnectAsync();
            }

            if (OWO.IsConnected)
            {
                suitDisabled = false;
                LOG("OWO suit connected.");
            }
            if (suitDisabled) LOG("Owo is not enabled?!?!");
        }

        ~TactsuitVR()
        {
            LOG("Destructor called");
            DisconnectOwo();
        }

        public void DisconnectOwo()
        {
            LOG("Disconnecting Owo skin.");
            OWO.Disconnect();
        }

        public void LOG(string logStr)
        {
            Plugin.Log.Info(logStr);
            //Plugin.Log.LogMessage(logStr);
        }

        void RegisterAllTactFiles()
        {
            if (suitDisabled) return;
            LOG("Registering tact files...");
            ResourceSet resourceSet = OwoFunctional.Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            foreach (DictionaryEntry d in resourceSet)
            {
                LOG("Pattern registered: " + d.Key.ToString());
                try
                {
                    ISensation test = Sensation.FromCode(d.Value.ToString());
                    FeedbackMap.Add(d.Key.ToString(), test);
                }
                catch (Exception e) { LOG(e.ToString()); }

            }
            systemInitialized = true;
        }


        public void Recoil(bool isRightHand)
        {
            if (isRightHand) PlayBackFeedback("RecoilBlade_R");
            else PlayBackFeedback("RecoilBlade_L");
        }

        public void PlayBackFeedback(string feedback)
        {
            if (FeedbackMap.ContainsKey(feedback))
            {
                OWO.Send(FeedbackMap[feedback]);
            }
            else LOG("Feedback not registered: " + feedback);
        }

    }
}
