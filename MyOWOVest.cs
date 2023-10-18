using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using OWOGame;
using OwoFunctional;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Net;

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
        public bool suitDisabled = false;
        public bool systemInitialized = false;
        // Event to start and stop the heartbeat thread
        public Dictionary<String, Sensation> FeedbackMap = new Dictionary<String, Sensation>();


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
            RegisterAllTactFiles();
            InitializeOWO();
        }

        private async void InitializeOWO()
        {
            LOG("Initializing suit");

            // New auth.
            var gameAuth = GameAuth.Create(AllBakedSensations()).WithId("85963451");

            OWO.Configure(gameAuth);
            string myIP = getIpFromFile("OWO_Manual_IP.txt");
            if (myIP == "") await OWO.AutoConnect();
            else
            {
                LOG("Found manual IP address: " + myIP);
                await OWO.Connect(myIP);
            }

            if (OWO.ConnectionState == ConnectionState.Connected)
            {
                suitDisabled = false;
                LOG("OWO suit connected.");
            }
            if (suitDisabled) LOG("Owo is not enabled?!?!");
        }

        private BakedSensation[] AllBakedSensations()
        {
            var result = new List<BakedSensation>();

            foreach (var sensation in FeedbackMap.Values)
            {
                if (sensation is BakedSensation baked)
                {
                    LOG("Registered baked sensation: " + baked.name);
                    result.Add(baked);
                }
                else
                {
                    LOG("Sensation not baked? " + sensation);
                    continue;
                }
            }
            return result.ToArray();
        }

        public string getIpFromFile(string filename)
        {
            string ip = "";
            string configPath = Path.Combine(IPA.Utilities.UnityGame.UserDataPath, filename);
            if (File.Exists(configPath))
            {
                string fileBuffer = File.ReadAllText(configPath);
                IPAddress address;
                if (IPAddress.TryParse(fileBuffer, out address)) ip = fileBuffer;
            }
            return ip;
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
                    Sensation test = Sensation.Parse(d.Value.ToString());
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
