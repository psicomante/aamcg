using UnityEngine;
using System.Collections;
using Amucuga;

public class NormalPowerUp : PowerUp {

    private Material _backupMaterial;
    private Shader _normalShader;

    public NormalPowerUp(Shader NormalShader) 
        :base(15)
    {
        _normalShader = NormalShader;
        PowerUpColor = new Color(0.8f,0.2f,0.6f);
        IsCumulative = false;
        Name = "Normal";
    }

    protected override void EnablePowerUpEffect()
    {
        _backupMaterial = AttachedPlayer.Cube.renderer.material;
        AttachedPlayer.Cube.renderer.material = new Material(_normalShader);
    }

    protected override void DisablePowerUpEffect()
    {
        AttachedPlayer.Cube.renderer.material = _backupMaterial;
    }
}
