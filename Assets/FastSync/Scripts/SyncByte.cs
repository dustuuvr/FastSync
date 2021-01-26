
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Dustuu.VRChat.FastSync
{
    public class SyncByte : UdonSharpBehaviour
    {
        // UdonSynced for late-joiners
        [UdonSynced]
        private int udonSynced;
        // Network Sync for all other usage
        private byte fastSynced;
        private bool fastSyncedFilled;
        private byte fastSyncedTemp;
        private bool fastSyncedTempFilled;

        public override void OnDeserialization()
        {
            if (!fastSyncedFilled && udonSynced != 0)
            {
                byte val = GetUdonSynced();
                SetFastSynced(val);
            }
        }

        public byte GetUdonSynced() { return (byte)udonSynced; }
        private void SetUdonSynced(byte udonSynced)
        {
            if (Networking.IsOwner(gameObject)) { this.udonSynced = udonSynced; }
        }

        public byte GetFastSynced() { return fastSynced; }
        private void SetFastSynced(byte fastSynced)
        {
            this.fastSynced = fastSynced;
            fastSyncedFilled = true;
            ClearTemp();
        }

        // Call this method to change the byte
        public void RequestByte(byte request)
        {
            if (request >= 0 && request <= 255) { SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, $"B{request}"); }
            else { Debug.LogError($"[FastSync] Requested byte {request} was not within valid byte range"); }
        }

        private void SetTemp(byte data)
        {
            fastSyncedTemp = data;
            fastSyncedTempFilled = true;
        }

        private void ClearTemp()
        {
            fastSyncedTemp = 0;
            fastSyncedTempFilled = false;
        }

        public void Convert()
        {
            if (fastSyncedTempFilled)
            {
                SetUdonSynced(fastSyncedTemp);
                SetFastSynced(fastSyncedTemp);
            }
            else { Debug.LogError("[FastSync] Attempted to convert fastSyncedTemp value that wasn't filled"); }
        }

        public void B0() { SetTemp(0); }
        public void B1() { SetTemp(1); }
        public void B2() { SetTemp(2); }
        public void B3() { SetTemp(3); }
        public void B4() { SetTemp(4); }
        public void B5() { SetTemp(5); }
        public void B6() { SetTemp(6); }
        public void B7() { SetTemp(7); }
        public void B8() { SetTemp(8); }
        public void B9() { SetTemp(9); }
        public void B10() { SetTemp(10); }
        public void B11() { SetTemp(11); }
        public void B12() { SetTemp(12); }
        public void B13() { SetTemp(13); }
        public void B14() { SetTemp(14); }
        public void B15() { SetTemp(15); }
        public void B16() { SetTemp(16); }
        public void B17() { SetTemp(17); }
        public void B18() { SetTemp(18); }
        public void B19() { SetTemp(19); }
        public void B20() { SetTemp(20); }
        public void B21() { SetTemp(21); }
        public void B22() { SetTemp(22); }
        public void B23() { SetTemp(23); }
        public void B24() { SetTemp(24); }
        public void B25() { SetTemp(25); }
        public void B26() { SetTemp(26); }
        public void B27() { SetTemp(27); }
        public void B28() { SetTemp(28); }
        public void B29() { SetTemp(29); }
        public void B30() { SetTemp(30); }
        public void B31() { SetTemp(31); }
        public void B32() { SetTemp(32); }
        public void B33() { SetTemp(33); }
        public void B34() { SetTemp(34); }
        public void B35() { SetTemp(35); }
        public void B36() { SetTemp(36); }
        public void B37() { SetTemp(37); }
        public void B38() { SetTemp(38); }
        public void B39() { SetTemp(39); }
        public void B40() { SetTemp(40); }
        public void B41() { SetTemp(41); }
        public void B42() { SetTemp(42); }
        public void B43() { SetTemp(43); }
        public void B44() { SetTemp(44); }
        public void B45() { SetTemp(45); }
        public void B46() { SetTemp(46); }
        public void B47() { SetTemp(47); }
        public void B48() { SetTemp(48); }
        public void B49() { SetTemp(49); }
        public void B50() { SetTemp(50); }
        public void B51() { SetTemp(51); }
        public void B52() { SetTemp(52); }
        public void B53() { SetTemp(53); }
        public void B54() { SetTemp(54); }
        public void B55() { SetTemp(55); }
        public void B56() { SetTemp(56); }
        public void B57() { SetTemp(57); }
        public void B58() { SetTemp(58); }
        public void B59() { SetTemp(59); }
        public void B60() { SetTemp(60); }
        public void B61() { SetTemp(61); }
        public void B62() { SetTemp(62); }
        public void B63() { SetTemp(63); }
        public void B64() { SetTemp(64); }
        public void B65() { SetTemp(65); }
        public void B66() { SetTemp(66); }
        public void B67() { SetTemp(67); }
        public void B68() { SetTemp(68); }
        public void B69() { SetTemp(69); }
        public void B70() { SetTemp(70); }
        public void B71() { SetTemp(71); }
        public void B72() { SetTemp(72); }
        public void B73() { SetTemp(73); }
        public void B74() { SetTemp(74); }
        public void B75() { SetTemp(75); }
        public void B76() { SetTemp(76); }
        public void B77() { SetTemp(77); }
        public void B78() { SetTemp(78); }
        public void B79() { SetTemp(79); }
        public void B80() { SetTemp(80); }
        public void B81() { SetTemp(81); }
        public void B82() { SetTemp(82); }
        public void B83() { SetTemp(83); }
        public void B84() { SetTemp(84); }
        public void B85() { SetTemp(85); }
        public void B86() { SetTemp(86); }
        public void B87() { SetTemp(87); }
        public void B88() { SetTemp(88); }
        public void B89() { SetTemp(89); }
        public void B90() { SetTemp(90); }
        public void B91() { SetTemp(91); }
        public void B92() { SetTemp(92); }
        public void B93() { SetTemp(93); }
        public void B94() { SetTemp(94); }
        public void B95() { SetTemp(95); }
        public void B96() { SetTemp(96); }
        public void B97() { SetTemp(97); }
        public void B98() { SetTemp(98); }
        public void B99() { SetTemp(99); }
        public void B100() { SetTemp(100); }
        public void B101() { SetTemp(101); }
        public void B102() { SetTemp(102); }
        public void B103() { SetTemp(103); }
        public void B104() { SetTemp(104); }
        public void B105() { SetTemp(105); }
        public void B106() { SetTemp(106); }
        public void B107() { SetTemp(107); }
        public void B108() { SetTemp(108); }
        public void B109() { SetTemp(109); }
        public void B110() { SetTemp(110); }
        public void B111() { SetTemp(111); }
        public void B112() { SetTemp(112); }
        public void B113() { SetTemp(113); }
        public void B114() { SetTemp(114); }
        public void B115() { SetTemp(115); }
        public void B116() { SetTemp(116); }
        public void B117() { SetTemp(117); }
        public void B118() { SetTemp(118); }
        public void B119() { SetTemp(119); }
        public void B120() { SetTemp(120); }
        public void B121() { SetTemp(121); }
        public void B122() { SetTemp(122); }
        public void B123() { SetTemp(123); }
        public void B124() { SetTemp(124); }
        public void B125() { SetTemp(125); }
        public void B126() { SetTemp(126); }
        public void B127() { SetTemp(127); }
        public void B128() { SetTemp(128); }
        public void B129() { SetTemp(129); }
        public void B130() { SetTemp(130); }
        public void B131() { SetTemp(131); }
        public void B132() { SetTemp(132); }
        public void B133() { SetTemp(133); }
        public void B134() { SetTemp(134); }
        public void B135() { SetTemp(135); }
        public void B136() { SetTemp(136); }
        public void B137() { SetTemp(137); }
        public void B138() { SetTemp(138); }
        public void B139() { SetTemp(139); }
        public void B140() { SetTemp(140); }
        public void B141() { SetTemp(141); }
        public void B142() { SetTemp(142); }
        public void B143() { SetTemp(143); }
        public void B144() { SetTemp(144); }
        public void B145() { SetTemp(145); }
        public void B146() { SetTemp(146); }
        public void B147() { SetTemp(147); }
        public void B148() { SetTemp(148); }
        public void B149() { SetTemp(149); }
        public void B150() { SetTemp(150); }
        public void B151() { SetTemp(151); }
        public void B152() { SetTemp(152); }
        public void B153() { SetTemp(153); }
        public void B154() { SetTemp(154); }
        public void B155() { SetTemp(155); }
        public void B156() { SetTemp(156); }
        public void B157() { SetTemp(157); }
        public void B158() { SetTemp(158); }
        public void B159() { SetTemp(159); }
        public void B160() { SetTemp(160); }
        public void B161() { SetTemp(161); }
        public void B162() { SetTemp(162); }
        public void B163() { SetTemp(163); }
        public void B164() { SetTemp(164); }
        public void B165() { SetTemp(165); }
        public void B166() { SetTemp(166); }
        public void B167() { SetTemp(167); }
        public void B168() { SetTemp(168); }
        public void B169() { SetTemp(169); }
        public void B170() { SetTemp(170); }
        public void B171() { SetTemp(171); }
        public void B172() { SetTemp(172); }
        public void B173() { SetTemp(173); }
        public void B174() { SetTemp(174); }
        public void B175() { SetTemp(175); }
        public void B176() { SetTemp(176); }
        public void B177() { SetTemp(177); }
        public void B178() { SetTemp(178); }
        public void B179() { SetTemp(179); }
        public void B180() { SetTemp(180); }
        public void B181() { SetTemp(181); }
        public void B182() { SetTemp(182); }
        public void B183() { SetTemp(183); }
        public void B184() { SetTemp(184); }
        public void B185() { SetTemp(185); }
        public void B186() { SetTemp(186); }
        public void B187() { SetTemp(187); }
        public void B188() { SetTemp(188); }
        public void B189() { SetTemp(189); }
        public void B190() { SetTemp(190); }
        public void B191() { SetTemp(191); }
        public void B192() { SetTemp(192); }
        public void B193() { SetTemp(193); }
        public void B194() { SetTemp(194); }
        public void B195() { SetTemp(195); }
        public void B196() { SetTemp(196); }
        public void B197() { SetTemp(197); }
        public void B198() { SetTemp(198); }
        public void B199() { SetTemp(199); }
        public void B200() { SetTemp(200); }
        public void B201() { SetTemp(201); }
        public void B202() { SetTemp(202); }
        public void B203() { SetTemp(203); }
        public void B204() { SetTemp(204); }
        public void B205() { SetTemp(205); }
        public void B206() { SetTemp(206); }
        public void B207() { SetTemp(207); }
        public void B208() { SetTemp(208); }
        public void B209() { SetTemp(209); }
        public void B210() { SetTemp(210); }
        public void B211() { SetTemp(211); }
        public void B212() { SetTemp(212); }
        public void B213() { SetTemp(213); }
        public void B214() { SetTemp(214); }
        public void B215() { SetTemp(215); }
        public void B216() { SetTemp(216); }
        public void B217() { SetTemp(217); }
        public void B218() { SetTemp(218); }
        public void B219() { SetTemp(219); }
        public void B220() { SetTemp(220); }
        public void B221() { SetTemp(221); }
        public void B222() { SetTemp(222); }
        public void B223() { SetTemp(223); }
        public void B224() { SetTemp(224); }
        public void B225() { SetTemp(225); }
        public void B226() { SetTemp(226); }
        public void B227() { SetTemp(227); }
        public void B228() { SetTemp(228); }
        public void B229() { SetTemp(229); }
        public void B230() { SetTemp(230); }
        public void B231() { SetTemp(231); }
        public void B232() { SetTemp(232); }
        public void B233() { SetTemp(233); }
        public void B234() { SetTemp(234); }
        public void B235() { SetTemp(235); }
        public void B236() { SetTemp(236); }
        public void B237() { SetTemp(237); }
        public void B238() { SetTemp(238); }
        public void B239() { SetTemp(239); }
        public void B240() { SetTemp(240); }
        public void B241() { SetTemp(241); }
        public void B242() { SetTemp(242); }
        public void B243() { SetTemp(243); }
        public void B244() { SetTemp(244); }
        public void B245() { SetTemp(245); }
        public void B246() { SetTemp(246); }
        public void B247() { SetTemp(247); }
        public void B248() { SetTemp(248); }
        public void B249() { SetTemp(249); }
        public void B250() { SetTemp(250); }
        public void B251() { SetTemp(251); }
        public void B252() { SetTemp(252); }
        public void B253() { SetTemp(253); }
        public void B254() { SetTemp(254); }
        public void B255() { SetTemp(255); }
    }
}