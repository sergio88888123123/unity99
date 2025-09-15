== Interactive Book 08: Detección de Colisiones (Implementación) ==

Incluido:
1) CollisionEvents2D.cs - UnityEvents para OnTrigger*/OnCollision* con filtros por capa/tag.
2) Rigidbody2DAttacher.cs (Editor) - Añade Rigidbody2D + BoxCollider2D si faltan.
3) OnCollisionActions2D.cs - Feedback visual/sonoro + eventos de score/damage.
4) Collectible2D.cs - Ejemplo de trigger para 'Player' que suma puntuación y se oculta.
5) RectOverlapDetectorUI.cs - 'Colisión' por solapamiento de RectTransforms en UI.

Cómo usar (rápido):
- Player: Rigidbody2D + Collider2D (tag 'Player').
- Ítems/Hazards: Collider2D (activar IsTrigger si quieres trigger).
- Añade CollisionEvents2D al objeto que quieres escuchar colisiones y vincula
  SetEnterFeedback/SetExitFeedback o AddScore/TakeDamage desde OnCollisionActions2D.
- Para UI, usa RectOverlapDetectorUI con los dos RectTransform a comparar.
