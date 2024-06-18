TAG : 
- RightHand
- LeftHand
- Meteor
- Nozzel
- SelangGulung
- SelangPanjang
- Handle
- Api
- Spawn
- SelangTirisanAir

Layer : 
1. Default
2. TransparentFX
3. Ignore Raycast
4. Water
5. UI
6. Grab
7. Player
8. SecondHandGrab
9. Socket
10. HornHydrant
11. SelangSocket
12. Invisible

Code : 

1. Api = Untuk diobject api berada dan memiliki fungsi base pemadaman HP api.
2. AssesmenManager = Untuk mengatur scene assesmen ketika mulai dan menampung player ketika belum siap.
3. HapticInteractible = Untuk mengecek listener player pada object tersebut.
4. PlayerNameManager = Untuk menampung data ketika player memasukan nama di lobby.
5. ScoreManager = Untuk mengatur pertambahan, pengurangan serta hasil dari score akhir pada assesmen scene.
6. TrainingManager = Untuk mengatur seluruh kondisi pada scene training dimana untuk next step UI.
7. TriggerApi = Untuk menampung kondisi agar destroy api ketika parent sudah null.
8. TriggerScore = Untuk trigger score kondisi pada assesmen scene.

Code/Hydant Selang : 

1. HandleHydrant : Untuk script handle hydrant object.
2. HydrantSelang : Untuk manager selang yang terpasang tanpa nozzel.
3. Nozzel : Untuk manager selang serta nozzel ketika sudah berada di pillar.
4. SwitchWaterMode = Untuk mengganti mode selang dari perisai ke memadamkan atau sebaliknya.
5. RandomMoveForSelangOnly = Untuk error kondisi di selang tanpa nozzel ketika sudah terpasang di pillar serta air menyala.
6. RandomMovement = Untuk error kondisi di selang dengan nozzel ketika sudah terpasang di pillar serta air menyala.
7. SpawnHandleHydrant = Untuk menspawn object baru handle, selang, dan nozzel ketika di copot dari pillar.
8. StatusWater = Untuk manager kondisi pillar dari nyala matinya air dan besar kecilnya air.
9. TiriskanSelang = Untuk manager selang tirisan air agar bisa nonaktif dan aktifkan grab serta finish tirisan air.
10. TrajectoryPredictor = Untuk mengatur besar kecilnya particle air yang dikirim StatusWater.
11. TwistHydrant = Untuk mengatur arah sesuai katup pillar ketika handle di ambil.
12. TwoHandSelang = Untuk scirpt selang gulung.
13. WaterDestory = Untuk mengurangi HP dari api.
14. WhellRotation = Untuk knob rotasi.