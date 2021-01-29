
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class FastSyncByte : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour[] fastSyncByteChangedSubscribers;
        [UdonSynced] private int dataUdonSynced = -1; // UdonSynced for late-joiners
        private int data = -1; // Network Sync for all other usage
        private FastSyncByteManager fastSyncByteManager;

        public override void OnDeserialization() { if (dataUdonSynced != -1 && data == -1) { SetData(dataUdonSynced); } }

        private void FastSyncByteChanged()
        {
            if (fastSyncByteChangedSubscribers == null) { return; }

            // Alert subscribers
            foreach (UdonSharpBehaviour fastSyncByteChangedSubscriber in fastSyncByteChangedSubscribers)
            { fastSyncByteChangedSubscriber.SendCustomEvent("OnFastSyncByteChanged"); }
        }

        // Call this method to change the byte
        public void RequestByte(byte request)
        {
            if (request >= 0 && request <= 255) { SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, $"B{request}"); }
            else { Debug.LogError($"[FastSync] FastSyncByte: Requested byte {request} was not within valid byte range"); }
        }

        public byte GetData() { return data == -1 ? (byte)0 : (byte)data; }
        private void SetData(int data)
        {
            this.data = data;
            if (Networking.IsOwner(gameObject)) { dataUdonSynced = GetData(); }
            if ( GetFastSyncByteManager() != null) { GetFastSyncByteManager().HandleChange(this); }
            FastSyncByteChanged();
        }

        public FastSyncByteManager GetFastSyncByteManager()
        { return fastSyncByteManager != null ? fastSyncByteManager : fastSyncByteManager = GetComponentInParent<FastSyncByteManager>(); }

        public void B0() { SetData(0); }
        public void B1() { SetData(1); }
        public void B2() { SetData(2); }
        public void B3() { SetData(3); }
        public void B4() { SetData(4); }
        public void B5() { SetData(5); }
        public void B6() { SetData(6); }
        public void B7() { SetData(7); }
        public void B8() { SetData(8); }
        public void B9() { SetData(9); }
        public void B10() { SetData(10); }
        public void B11() { SetData(11); }
        public void B12() { SetData(12); }
        public void B13() { SetData(13); }
        public void B14() { SetData(14); }
        public void B15() { SetData(15); }
        public void B16() { SetData(16); }
        public void B17() { SetData(17); }
        public void B18() { SetData(18); }
        public void B19() { SetData(19); }
        public void B20() { SetData(20); }
        public void B21() { SetData(21); }
        public void B22() { SetData(22); }
        public void B23() { SetData(23); }
        public void B24() { SetData(24); }
        public void B25() { SetData(25); }
        public void B26() { SetData(26); }
        public void B27() { SetData(27); }
        public void B28() { SetData(28); }
        public void B29() { SetData(29); }
        public void B30() { SetData(30); }
        public void B31() { SetData(31); }
        public void B32() { SetData(32); }
        public void B33() { SetData(33); }
        public void B34() { SetData(34); }
        public void B35() { SetData(35); }
        public void B36() { SetData(36); }
        public void B37() { SetData(37); }
        public void B38() { SetData(38); }
        public void B39() { SetData(39); }
        public void B40() { SetData(40); }
        public void B41() { SetData(41); }
        public void B42() { SetData(42); }
        public void B43() { SetData(43); }
        public void B44() { SetData(44); }
        public void B45() { SetData(45); }
        public void B46() { SetData(46); }
        public void B47() { SetData(47); }
        public void B48() { SetData(48); }
        public void B49() { SetData(49); }
        public void B50() { SetData(50); }
        public void B51() { SetData(51); }
        public void B52() { SetData(52); }
        public void B53() { SetData(53); }
        public void B54() { SetData(54); }
        public void B55() { SetData(55); }
        public void B56() { SetData(56); }
        public void B57() { SetData(57); }
        public void B58() { SetData(58); }
        public void B59() { SetData(59); }
        public void B60() { SetData(60); }
        public void B61() { SetData(61); }
        public void B62() { SetData(62); }
        public void B63() { SetData(63); }
        public void B64() { SetData(64); }
        public void B65() { SetData(65); }
        public void B66() { SetData(66); }
        public void B67() { SetData(67); }
        public void B68() { SetData(68); }
        public void B69() { SetData(69); }
        public void B70() { SetData(70); }
        public void B71() { SetData(71); }
        public void B72() { SetData(72); }
        public void B73() { SetData(73); }
        public void B74() { SetData(74); }
        public void B75() { SetData(75); }
        public void B76() { SetData(76); }
        public void B77() { SetData(77); }
        public void B78() { SetData(78); }
        public void B79() { SetData(79); }
        public void B80() { SetData(80); }
        public void B81() { SetData(81); }
        public void B82() { SetData(82); }
        public void B83() { SetData(83); }
        public void B84() { SetData(84); }
        public void B85() { SetData(85); }
        public void B86() { SetData(86); }
        public void B87() { SetData(87); }
        public void B88() { SetData(88); }
        public void B89() { SetData(89); }
        public void B90() { SetData(90); }
        public void B91() { SetData(91); }
        public void B92() { SetData(92); }
        public void B93() { SetData(93); }
        public void B94() { SetData(94); }
        public void B95() { SetData(95); }
        public void B96() { SetData(96); }
        public void B97() { SetData(97); }
        public void B98() { SetData(98); }
        public void B99() { SetData(99); }
        public void B100() { SetData(100); }
        public void B101() { SetData(101); }
        public void B102() { SetData(102); }
        public void B103() { SetData(103); }
        public void B104() { SetData(104); }
        public void B105() { SetData(105); }
        public void B106() { SetData(106); }
        public void B107() { SetData(107); }
        public void B108() { SetData(108); }
        public void B109() { SetData(109); }
        public void B110() { SetData(110); }
        public void B111() { SetData(111); }
        public void B112() { SetData(112); }
        public void B113() { SetData(113); }
        public void B114() { SetData(114); }
        public void B115() { SetData(115); }
        public void B116() { SetData(116); }
        public void B117() { SetData(117); }
        public void B118() { SetData(118); }
        public void B119() { SetData(119); }
        public void B120() { SetData(120); }
        public void B121() { SetData(121); }
        public void B122() { SetData(122); }
        public void B123() { SetData(123); }
        public void B124() { SetData(124); }
        public void B125() { SetData(125); }
        public void B126() { SetData(126); }
        public void B127() { SetData(127); }
        public void B128() { SetData(128); }
        public void B129() { SetData(129); }
        public void B130() { SetData(130); }
        public void B131() { SetData(131); }
        public void B132() { SetData(132); }
        public void B133() { SetData(133); }
        public void B134() { SetData(134); }
        public void B135() { SetData(135); }
        public void B136() { SetData(136); }
        public void B137() { SetData(137); }
        public void B138() { SetData(138); }
        public void B139() { SetData(139); }
        public void B140() { SetData(140); }
        public void B141() { SetData(141); }
        public void B142() { SetData(142); }
        public void B143() { SetData(143); }
        public void B144() { SetData(144); }
        public void B145() { SetData(145); }
        public void B146() { SetData(146); }
        public void B147() { SetData(147); }
        public void B148() { SetData(148); }
        public void B149() { SetData(149); }
        public void B150() { SetData(150); }
        public void B151() { SetData(151); }
        public void B152() { SetData(152); }
        public void B153() { SetData(153); }
        public void B154() { SetData(154); }
        public void B155() { SetData(155); }
        public void B156() { SetData(156); }
        public void B157() { SetData(157); }
        public void B158() { SetData(158); }
        public void B159() { SetData(159); }
        public void B160() { SetData(160); }
        public void B161() { SetData(161); }
        public void B162() { SetData(162); }
        public void B163() { SetData(163); }
        public void B164() { SetData(164); }
        public void B165() { SetData(165); }
        public void B166() { SetData(166); }
        public void B167() { SetData(167); }
        public void B168() { SetData(168); }
        public void B169() { SetData(169); }
        public void B170() { SetData(170); }
        public void B171() { SetData(171); }
        public void B172() { SetData(172); }
        public void B173() { SetData(173); }
        public void B174() { SetData(174); }
        public void B175() { SetData(175); }
        public void B176() { SetData(176); }
        public void B177() { SetData(177); }
        public void B178() { SetData(178); }
        public void B179() { SetData(179); }
        public void B180() { SetData(180); }
        public void B181() { SetData(181); }
        public void B182() { SetData(182); }
        public void B183() { SetData(183); }
        public void B184() { SetData(184); }
        public void B185() { SetData(185); }
        public void B186() { SetData(186); }
        public void B187() { SetData(187); }
        public void B188() { SetData(188); }
        public void B189() { SetData(189); }
        public void B190() { SetData(190); }
        public void B191() { SetData(191); }
        public void B192() { SetData(192); }
        public void B193() { SetData(193); }
        public void B194() { SetData(194); }
        public void B195() { SetData(195); }
        public void B196() { SetData(196); }
        public void B197() { SetData(197); }
        public void B198() { SetData(198); }
        public void B199() { SetData(199); }
        public void B200() { SetData(200); }
        public void B201() { SetData(201); }
        public void B202() { SetData(202); }
        public void B203() { SetData(203); }
        public void B204() { SetData(204); }
        public void B205() { SetData(205); }
        public void B206() { SetData(206); }
        public void B207() { SetData(207); }
        public void B208() { SetData(208); }
        public void B209() { SetData(209); }
        public void B210() { SetData(210); }
        public void B211() { SetData(211); }
        public void B212() { SetData(212); }
        public void B213() { SetData(213); }
        public void B214() { SetData(214); }
        public void B215() { SetData(215); }
        public void B216() { SetData(216); }
        public void B217() { SetData(217); }
        public void B218() { SetData(218); }
        public void B219() { SetData(219); }
        public void B220() { SetData(220); }
        public void B221() { SetData(221); }
        public void B222() { SetData(222); }
        public void B223() { SetData(223); }
        public void B224() { SetData(224); }
        public void B225() { SetData(225); }
        public void B226() { SetData(226); }
        public void B227() { SetData(227); }
        public void B228() { SetData(228); }
        public void B229() { SetData(229); }
        public void B230() { SetData(230); }
        public void B231() { SetData(231); }
        public void B232() { SetData(232); }
        public void B233() { SetData(233); }
        public void B234() { SetData(234); }
        public void B235() { SetData(235); }
        public void B236() { SetData(236); }
        public void B237() { SetData(237); }
        public void B238() { SetData(238); }
        public void B239() { SetData(239); }
        public void B240() { SetData(240); }
        public void B241() { SetData(241); }
        public void B242() { SetData(242); }
        public void B243() { SetData(243); }
        public void B244() { SetData(244); }
        public void B245() { SetData(245); }
        public void B246() { SetData(246); }
        public void B247() { SetData(247); }
        public void B248() { SetData(248); }
        public void B249() { SetData(249); }
        public void B250() { SetData(250); }
        public void B251() { SetData(251); }
        public void B252() { SetData(252); }
        public void B253() { SetData(253); }
        public void B254() { SetData(254); }
        public void B255() { SetData(255); }
    }
}